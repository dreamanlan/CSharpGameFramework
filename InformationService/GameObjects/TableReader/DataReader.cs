//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由TableReaderGenerator按table.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using GameFramework;

namespace TableConfig
{
	public sealed partial class Actor : IDataRecord<int>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 188)]
		private struct ActorRecord
		{
			internal int id;
			internal int name;
			internal int icon;
			internal int bigIcon;
			internal int type;
			internal int avatar;
			internal int skill0;
			internal int skill1;
			internal int skill2;
			internal int skill3;
			internal int skill4;
			internal int skill5;
			internal int skill6;
			internal int skill7;
			internal int skill8;
			internal int bornskill;
			internal int deadskill;
			internal float size;
			internal float logicsize;
			internal float speed;
			internal float viewrange;
			internal float gohomerange;
			internal int x4001;
			internal int x4002;
			internal int x4003;
			internal int x4004;
			internal int x1001;
			internal int x1002;
			internal int x1006;
			internal int x1007;
			internal int x1011;
			internal int x1012;
			internal int x1016;
			internal int x1017;
			internal int x1021;
			internal int x1022;
			internal int x1024;
			internal int x1026;
			internal int x1028;
			internal int x1030;
			internal int x1032;
			internal int x1033;
			internal int x1034;
			internal int x2001;
			internal int x2002;
			internal int x2007;
			internal int x2008;
		}

		public int id;
		public string name;
		public int icon;
		public int bigIcon;
		public int type;
		public string avatar;
		public int skill0;
		public int skill1;
		public int skill2;
		public int skill3;
		public int skill4;
		public int skill5;
		public int skill6;
		public int skill7;
		public int skill8;
		public int bornskill;
		public int deadskill;
		public float size;
		public float logicsize;
		public float speed;
		public float viewrange;
		public float gohomerange;
		public int x4001;
		public int x4002;
		public int x4003;
		public int x4004;
		public int x1001;
		public int x1002;
		public int x1006;
		public int x1007;
		public int x1011;
		public int x1012;
		public int x1016;
		public int x1017;
		public int x1021;
		public int x1022;
		public int x1024;
		public int x1026;
		public int x1028;
		public int x1030;
		public int x1032;
		public int x1033;
		public int x1034;
		public int x2001;
		public int x2002;
		public int x2007;
		public int x2008;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			ActorRecord record = GetRecord(table,index);
			id = DataRecordUtility.ExtractInt(table, record.id, 0);
			name = DataRecordUtility.ExtractString(table, record.name, "");
			icon = DataRecordUtility.ExtractInt(table, record.icon, 0);
			bigIcon = DataRecordUtility.ExtractInt(table, record.bigIcon, 0);
			type = DataRecordUtility.ExtractInt(table, record.type, 0);
			avatar = DataRecordUtility.ExtractString(table, record.avatar, "");
			skill0 = DataRecordUtility.ExtractInt(table, record.skill0, 0);
			skill1 = DataRecordUtility.ExtractInt(table, record.skill1, 0);
			skill2 = DataRecordUtility.ExtractInt(table, record.skill2, 0);
			skill3 = DataRecordUtility.ExtractInt(table, record.skill3, 0);
			skill4 = DataRecordUtility.ExtractInt(table, record.skill4, 0);
			skill5 = DataRecordUtility.ExtractInt(table, record.skill5, 0);
			skill6 = DataRecordUtility.ExtractInt(table, record.skill6, 0);
			skill7 = DataRecordUtility.ExtractInt(table, record.skill7, 0);
			skill8 = DataRecordUtility.ExtractInt(table, record.skill8, 0);
			bornskill = DataRecordUtility.ExtractInt(table, record.bornskill, 0);
			deadskill = DataRecordUtility.ExtractInt(table, record.deadskill, 0);
			size = DataRecordUtility.ExtractFloat(table, record.size, 0);
			logicsize = DataRecordUtility.ExtractFloat(table, record.logicsize, 0);
			speed = DataRecordUtility.ExtractFloat(table, record.speed, 0);
			viewrange = DataRecordUtility.ExtractFloat(table, record.viewrange, 0);
			gohomerange = DataRecordUtility.ExtractFloat(table, record.gohomerange, 0);
			x4001 = DataRecordUtility.ExtractInt(table, record.x4001, 0);
			x4002 = DataRecordUtility.ExtractInt(table, record.x4002, 0);
			x4003 = DataRecordUtility.ExtractInt(table, record.x4003, 0);
			x4004 = DataRecordUtility.ExtractInt(table, record.x4004, 0);
			x1001 = DataRecordUtility.ExtractInt(table, record.x1001, 0);
			x1002 = DataRecordUtility.ExtractInt(table, record.x1002, 0);
			x1006 = DataRecordUtility.ExtractInt(table, record.x1006, 0);
			x1007 = DataRecordUtility.ExtractInt(table, record.x1007, 0);
			x1011 = DataRecordUtility.ExtractInt(table, record.x1011, 0);
			x1012 = DataRecordUtility.ExtractInt(table, record.x1012, 0);
			x1016 = DataRecordUtility.ExtractInt(table, record.x1016, 0);
			x1017 = DataRecordUtility.ExtractInt(table, record.x1017, 0);
			x1021 = DataRecordUtility.ExtractInt(table, record.x1021, 0);
			x1022 = DataRecordUtility.ExtractInt(table, record.x1022, 0);
			x1024 = DataRecordUtility.ExtractInt(table, record.x1024, 0);
			x1026 = DataRecordUtility.ExtractInt(table, record.x1026, 0);
			x1028 = DataRecordUtility.ExtractInt(table, record.x1028, 0);
			x1030 = DataRecordUtility.ExtractInt(table, record.x1030, 0);
			x1032 = DataRecordUtility.ExtractInt(table, record.x1032, 0);
			x1033 = DataRecordUtility.ExtractInt(table, record.x1033, 0);
			x1034 = DataRecordUtility.ExtractInt(table, record.x1034, 0);
			x2001 = DataRecordUtility.ExtractInt(table, record.x2001, 0);
			x2002 = DataRecordUtility.ExtractInt(table, record.x2002, 0);
			x2007 = DataRecordUtility.ExtractInt(table, record.x2007, 0);
			x2008 = DataRecordUtility.ExtractInt(table, record.x2008, 0);
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			ActorRecord record = new ActorRecord();
			record.id = DataRecordUtility.SetValue(table, id, 0);
			record.name = DataRecordUtility.SetValue(table, name, "");
			record.icon = DataRecordUtility.SetValue(table, icon, 0);
			record.bigIcon = DataRecordUtility.SetValue(table, bigIcon, 0);
			record.type = DataRecordUtility.SetValue(table, type, 0);
			record.avatar = DataRecordUtility.SetValue(table, avatar, "");
			record.skill0 = DataRecordUtility.SetValue(table, skill0, 0);
			record.skill1 = DataRecordUtility.SetValue(table, skill1, 0);
			record.skill2 = DataRecordUtility.SetValue(table, skill2, 0);
			record.skill3 = DataRecordUtility.SetValue(table, skill3, 0);
			record.skill4 = DataRecordUtility.SetValue(table, skill4, 0);
			record.skill5 = DataRecordUtility.SetValue(table, skill5, 0);
			record.skill6 = DataRecordUtility.SetValue(table, skill6, 0);
			record.skill7 = DataRecordUtility.SetValue(table, skill7, 0);
			record.skill8 = DataRecordUtility.SetValue(table, skill8, 0);
			record.bornskill = DataRecordUtility.SetValue(table, bornskill, 0);
			record.deadskill = DataRecordUtility.SetValue(table, deadskill, 0);
			record.size = DataRecordUtility.SetValue(table, size, 0);
			record.logicsize = DataRecordUtility.SetValue(table, logicsize, 0);
			record.speed = DataRecordUtility.SetValue(table, speed, 0);
			record.viewrange = DataRecordUtility.SetValue(table, viewrange, 0);
			record.gohomerange = DataRecordUtility.SetValue(table, gohomerange, 0);
			record.x4001 = DataRecordUtility.SetValue(table, x4001, 0);
			record.x4002 = DataRecordUtility.SetValue(table, x4002, 0);
			record.x4003 = DataRecordUtility.SetValue(table, x4003, 0);
			record.x4004 = DataRecordUtility.SetValue(table, x4004, 0);
			record.x1001 = DataRecordUtility.SetValue(table, x1001, 0);
			record.x1002 = DataRecordUtility.SetValue(table, x1002, 0);
			record.x1006 = DataRecordUtility.SetValue(table, x1006, 0);
			record.x1007 = DataRecordUtility.SetValue(table, x1007, 0);
			record.x1011 = DataRecordUtility.SetValue(table, x1011, 0);
			record.x1012 = DataRecordUtility.SetValue(table, x1012, 0);
			record.x1016 = DataRecordUtility.SetValue(table, x1016, 0);
			record.x1017 = DataRecordUtility.SetValue(table, x1017, 0);
			record.x1021 = DataRecordUtility.SetValue(table, x1021, 0);
			record.x1022 = DataRecordUtility.SetValue(table, x1022, 0);
			record.x1024 = DataRecordUtility.SetValue(table, x1024, 0);
			record.x1026 = DataRecordUtility.SetValue(table, x1026, 0);
			record.x1028 = DataRecordUtility.SetValue(table, x1028, 0);
			record.x1030 = DataRecordUtility.SetValue(table, x1030, 0);
			record.x1032 = DataRecordUtility.SetValue(table, x1032, 0);
			record.x1033 = DataRecordUtility.SetValue(table, x1033, 0);
			record.x1034 = DataRecordUtility.SetValue(table, x1034, 0);
			record.x2001 = DataRecordUtility.SetValue(table, x2001, 0);
			record.x2002 = DataRecordUtility.SetValue(table, x2002, 0);
			record.x2007 = DataRecordUtility.SetValue(table, x2007, 0);
			record.x2008 = DataRecordUtility.SetValue(table, x2008, 0);
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		public int GetId()
		{
			return id;
		}

		private unsafe ActorRecord GetRecord(BinaryTable table, int index)
		{
			ActorRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(ActorRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(ActorRecord record)
		{
			byte[] bytes = new byte[sizeof(ActorRecord)];
			fixed (byte* p = bytes) {
				ActorRecord* temp = (ActorRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class ActorProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_Actor);
		}
		public void LoadForServer()
		{
			Load(FilePathDefine_Server.C_Actor);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_ActorMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("Actor is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_ActorMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_ActorMgr.Clear();
		}

		public DataDictionaryMgr<Actor,int> ActorMgr
		{
			get { return m_ActorMgr; }
		}

		public int GetActorCount()
		{
			return m_ActorMgr.GetDataCount();
		}

		public Actor GetActor(int id)
		{
			return m_ActorMgr.GetDataById(id);
		}

		private DataDictionaryMgr<Actor,int> m_ActorMgr = new DataDictionaryMgr<Actor,int>();

		public static ActorProvider Instance
		{
			get { return s_Instance; }
		}
		private static ActorProvider s_Instance = new ActorProvider();
	}
}

namespace TableConfig
{
	public sealed partial class AttrDefine : IDataRecord<int>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 16)]
		private struct AttrDefineRecord
		{
			internal int id;
			internal int value;
			internal int minValue;
			internal int maxValue;
		}

		public int id;
		public int value;
		public int minValue;
		public int maxValue;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			AttrDefineRecord record = GetRecord(table,index);
			id = DataRecordUtility.ExtractInt(table, record.id, 0);
			value = DataRecordUtility.ExtractInt(table, record.value, 0);
			minValue = DataRecordUtility.ExtractInt(table, record.minValue, 0);
			maxValue = DataRecordUtility.ExtractInt(table, record.maxValue, 0);
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			AttrDefineRecord record = new AttrDefineRecord();
			record.id = DataRecordUtility.SetValue(table, id, 0);
			record.value = DataRecordUtility.SetValue(table, value, 0);
			record.minValue = DataRecordUtility.SetValue(table, minValue, 0);
			record.maxValue = DataRecordUtility.SetValue(table, maxValue, 0);
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		public int GetId()
		{
			return id;
		}

		private unsafe AttrDefineRecord GetRecord(BinaryTable table, int index)
		{
			AttrDefineRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(AttrDefineRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(AttrDefineRecord record)
		{
			byte[] bytes = new byte[sizeof(AttrDefineRecord)];
			fixed (byte* p = bytes) {
				AttrDefineRecord* temp = (AttrDefineRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class AttrDefineProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_AttrDefine);
		}
		public void LoadForServer()
		{
			Load(FilePathDefine_Server.C_AttrDefine);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_AttrDefineMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("AttrDefine is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_AttrDefineMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_AttrDefineMgr.Clear();
		}

		public DataDictionaryMgr<AttrDefine,int> AttrDefineMgr
		{
			get { return m_AttrDefineMgr; }
		}

		public int GetAttrDefineCount()
		{
			return m_AttrDefineMgr.GetDataCount();
		}

		public AttrDefine GetAttrDefine(int id)
		{
			return m_AttrDefineMgr.GetDataById(id);
		}

		private DataDictionaryMgr<AttrDefine,int> m_AttrDefineMgr = new DataDictionaryMgr<AttrDefine,int>();

		public static AttrDefineProvider Instance
		{
			get { return s_Instance; }
		}
		private static AttrDefineProvider s_Instance = new AttrDefineProvider();
	}
}

namespace TableConfig
{
	public sealed partial class Const : IDataRecord<int>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 8)]
		private struct ConstRecord
		{
			internal int id;
			internal int value;
		}

		public int id;
		public int value;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			ConstRecord record = GetRecord(table,index);
			id = DataRecordUtility.ExtractInt(table, record.id, 0);
			value = DataRecordUtility.ExtractInt(table, record.value, 0);
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			ConstRecord record = new ConstRecord();
			record.id = DataRecordUtility.SetValue(table, id, 0);
			record.value = DataRecordUtility.SetValue(table, value, 0);
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		public int GetId()
		{
			return id;
		}

		private unsafe ConstRecord GetRecord(BinaryTable table, int index)
		{
			ConstRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(ConstRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(ConstRecord record)
		{
			byte[] bytes = new byte[sizeof(ConstRecord)];
			fixed (byte* p = bytes) {
				ConstRecord* temp = (ConstRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class ConstProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_Const);
		}
		public void LoadForServer()
		{
			Load(FilePathDefine_Server.C_Const);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_ConstMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("Const is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_ConstMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_ConstMgr.Clear();
		}

		public DataDictionaryMgr<Const,int> ConstMgr
		{
			get { return m_ConstMgr; }
		}

		public int GetConstCount()
		{
			return m_ConstMgr.GetDataCount();
		}

		public Const GetConst(int id)
		{
			return m_ConstMgr.GetDataById(id);
		}

		private DataDictionaryMgr<Const,int> m_ConstMgr = new DataDictionaryMgr<Const,int>();

		public static ConstProvider Instance
		{
			get { return s_Instance; }
		}
		private static ConstProvider s_Instance = new ConstProvider();
	}
}

namespace TableConfig
{
	public sealed partial class Formation : IDataRecord<int>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 132)]
		private struct FormationRecord
		{
			internal int teamid;
			internal int pos0;
			internal float dir0;
			internal int pos1;
			internal float dir1;
			internal int pos2;
			internal float dir2;
			internal int pos3;
			internal float dir3;
			internal int pos4;
			internal float dir4;
			internal int pos5;
			internal float dir5;
			internal int pos6;
			internal float dir6;
			internal int pos7;
			internal float dir7;
			internal int pos8;
			internal float dir8;
			internal int pos9;
			internal float dir9;
			internal int pos10;
			internal float dir10;
			internal int pos11;
			internal float dir11;
			internal int pos12;
			internal float dir12;
			internal int pos13;
			internal float dir13;
			internal int pos14;
			internal float dir14;
			internal int pos15;
			internal float dir15;
		}

		public int teamid;
		public List<float> pos0;
		public float dir0;
		public List<float> pos1;
		public float dir1;
		public List<float> pos2;
		public float dir2;
		public List<float> pos3;
		public float dir3;
		public List<float> pos4;
		public float dir4;
		public List<float> pos5;
		public float dir5;
		public List<float> pos6;
		public float dir6;
		public List<float> pos7;
		public float dir7;
		public List<float> pos8;
		public float dir8;
		public List<float> pos9;
		public float dir9;
		public List<float> pos10;
		public float dir10;
		public List<float> pos11;
		public float dir11;
		public List<float> pos12;
		public float dir12;
		public List<float> pos13;
		public float dir13;
		public List<float> pos14;
		public float dir14;
		public List<float> pos15;
		public float dir15;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			FormationRecord record = GetRecord(table,index);
			teamid = DataRecordUtility.ExtractInt(table, record.teamid, 0);
			pos0 = DataRecordUtility.ExtractFloatList(table, record.pos0, null);
			dir0 = DataRecordUtility.ExtractFloat(table, record.dir0, 0);
			pos1 = DataRecordUtility.ExtractFloatList(table, record.pos1, null);
			dir1 = DataRecordUtility.ExtractFloat(table, record.dir1, 0);
			pos2 = DataRecordUtility.ExtractFloatList(table, record.pos2, null);
			dir2 = DataRecordUtility.ExtractFloat(table, record.dir2, 0);
			pos3 = DataRecordUtility.ExtractFloatList(table, record.pos3, null);
			dir3 = DataRecordUtility.ExtractFloat(table, record.dir3, 0);
			pos4 = DataRecordUtility.ExtractFloatList(table, record.pos4, null);
			dir4 = DataRecordUtility.ExtractFloat(table, record.dir4, 0);
			pos5 = DataRecordUtility.ExtractFloatList(table, record.pos5, null);
			dir5 = DataRecordUtility.ExtractFloat(table, record.dir5, 0);
			pos6 = DataRecordUtility.ExtractFloatList(table, record.pos6, null);
			dir6 = DataRecordUtility.ExtractFloat(table, record.dir6, 0);
			pos7 = DataRecordUtility.ExtractFloatList(table, record.pos7, null);
			dir7 = DataRecordUtility.ExtractFloat(table, record.dir7, 0);
			pos8 = DataRecordUtility.ExtractFloatList(table, record.pos8, null);
			dir8 = DataRecordUtility.ExtractFloat(table, record.dir8, 0);
			pos9 = DataRecordUtility.ExtractFloatList(table, record.pos9, null);
			dir9 = DataRecordUtility.ExtractFloat(table, record.dir9, 0);
			pos10 = DataRecordUtility.ExtractFloatList(table, record.pos10, null);
			dir10 = DataRecordUtility.ExtractFloat(table, record.dir10, 0);
			pos11 = DataRecordUtility.ExtractFloatList(table, record.pos11, null);
			dir11 = DataRecordUtility.ExtractFloat(table, record.dir11, 0);
			pos12 = DataRecordUtility.ExtractFloatList(table, record.pos12, null);
			dir12 = DataRecordUtility.ExtractFloat(table, record.dir12, 0);
			pos13 = DataRecordUtility.ExtractFloatList(table, record.pos13, null);
			dir13 = DataRecordUtility.ExtractFloat(table, record.dir13, 0);
			pos14 = DataRecordUtility.ExtractFloatList(table, record.pos14, null);
			dir14 = DataRecordUtility.ExtractFloat(table, record.dir14, 0);
			pos15 = DataRecordUtility.ExtractFloatList(table, record.pos15, null);
			dir15 = DataRecordUtility.ExtractFloat(table, record.dir15, 0);
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			FormationRecord record = new FormationRecord();
			record.teamid = DataRecordUtility.SetValue(table, teamid, 0);
			record.pos0 = DataRecordUtility.SetValue(table, pos0, null);
			record.dir0 = DataRecordUtility.SetValue(table, dir0, 0);
			record.pos1 = DataRecordUtility.SetValue(table, pos1, null);
			record.dir1 = DataRecordUtility.SetValue(table, dir1, 0);
			record.pos2 = DataRecordUtility.SetValue(table, pos2, null);
			record.dir2 = DataRecordUtility.SetValue(table, dir2, 0);
			record.pos3 = DataRecordUtility.SetValue(table, pos3, null);
			record.dir3 = DataRecordUtility.SetValue(table, dir3, 0);
			record.pos4 = DataRecordUtility.SetValue(table, pos4, null);
			record.dir4 = DataRecordUtility.SetValue(table, dir4, 0);
			record.pos5 = DataRecordUtility.SetValue(table, pos5, null);
			record.dir5 = DataRecordUtility.SetValue(table, dir5, 0);
			record.pos6 = DataRecordUtility.SetValue(table, pos6, null);
			record.dir6 = DataRecordUtility.SetValue(table, dir6, 0);
			record.pos7 = DataRecordUtility.SetValue(table, pos7, null);
			record.dir7 = DataRecordUtility.SetValue(table, dir7, 0);
			record.pos8 = DataRecordUtility.SetValue(table, pos8, null);
			record.dir8 = DataRecordUtility.SetValue(table, dir8, 0);
			record.pos9 = DataRecordUtility.SetValue(table, pos9, null);
			record.dir9 = DataRecordUtility.SetValue(table, dir9, 0);
			record.pos10 = DataRecordUtility.SetValue(table, pos10, null);
			record.dir10 = DataRecordUtility.SetValue(table, dir10, 0);
			record.pos11 = DataRecordUtility.SetValue(table, pos11, null);
			record.dir11 = DataRecordUtility.SetValue(table, dir11, 0);
			record.pos12 = DataRecordUtility.SetValue(table, pos12, null);
			record.dir12 = DataRecordUtility.SetValue(table, dir12, 0);
			record.pos13 = DataRecordUtility.SetValue(table, pos13, null);
			record.dir13 = DataRecordUtility.SetValue(table, dir13, 0);
			record.pos14 = DataRecordUtility.SetValue(table, pos14, null);
			record.dir14 = DataRecordUtility.SetValue(table, dir14, 0);
			record.pos15 = DataRecordUtility.SetValue(table, pos15, null);
			record.dir15 = DataRecordUtility.SetValue(table, dir15, 0);
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		public int GetId()
		{
			return teamid;
		}

		private unsafe FormationRecord GetRecord(BinaryTable table, int index)
		{
			FormationRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(FormationRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(FormationRecord record)
		{
			byte[] bytes = new byte[sizeof(FormationRecord)];
			fixed (byte* p = bytes) {
				FormationRecord* temp = (FormationRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class FormationProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_Formation);
		}
		public void LoadForServer()
		{
			Load(FilePathDefine_Server.C_Formation);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_FormationMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("Formation is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_FormationMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_FormationMgr.Clear();
		}

		public DataDictionaryMgr<Formation,int> FormationMgr
		{
			get { return m_FormationMgr; }
		}

		public int GetFormationCount()
		{
			return m_FormationMgr.GetDataCount();
		}

		public Formation GetFormation(int id)
		{
			return m_FormationMgr.GetDataById(id);
		}

		private DataDictionaryMgr<Formation,int> m_FormationMgr = new DataDictionaryMgr<Formation,int>();

		public static FormationProvider Instance
		{
			get { return s_Instance; }
		}
		private static FormationProvider s_Instance = new FormationProvider();
	}
}

namespace TableConfig
{
	public sealed partial class ImpactData : IDataRecord<int>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 100)]
		private struct ImpactDataRecord
		{
			internal int id;
			internal int desc;
			internal int type;
			internal int icon;
			internal int duration;
			internal int cooldown;
			internal int multiple;
			internal int damage;
			internal int vampire;
			internal int attr1;
			internal int value1;
			internal int attr2;
			internal int value2;
			internal int attr3;
			internal int value3;
			internal int attr4;
			internal int value4;
			internal int attr5;
			internal int value5;
			internal int attr6;
			internal int value6;
			internal int attr7;
			internal int value7;
			internal int attr8;
			internal int value8;
		}

		public int id;
		public string desc;
		public int type;
		public int icon;
		public int duration;
		public int cooldown;
		public List<int> multiple;
		public List<int> damage;
		public List<int> vampire;
		public int attr1;
		public int value1;
		public int attr2;
		public int value2;
		public int attr3;
		public int value3;
		public int attr4;
		public int value4;
		public int attr5;
		public int value5;
		public int attr6;
		public int value6;
		public int attr7;
		public int value7;
		public int attr8;
		public int value8;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			ImpactDataRecord record = GetRecord(table,index);
			id = DataRecordUtility.ExtractInt(table, record.id, 0);
			desc = DataRecordUtility.ExtractString(table, record.desc, "");
			type = DataRecordUtility.ExtractInt(table, record.type, 0);
			icon = DataRecordUtility.ExtractInt(table, record.icon, 0);
			duration = DataRecordUtility.ExtractInt(table, record.duration, 0);
			cooldown = DataRecordUtility.ExtractInt(table, record.cooldown, 0);
			multiple = DataRecordUtility.ExtractIntList(table, record.multiple, null);
			damage = DataRecordUtility.ExtractIntList(table, record.damage, null);
			vampire = DataRecordUtility.ExtractIntList(table, record.vampire, null);
			attr1 = DataRecordUtility.ExtractInt(table, record.attr1, 0);
			value1 = DataRecordUtility.ExtractInt(table, record.value1, 0);
			attr2 = DataRecordUtility.ExtractInt(table, record.attr2, 0);
			value2 = DataRecordUtility.ExtractInt(table, record.value2, 0);
			attr3 = DataRecordUtility.ExtractInt(table, record.attr3, 0);
			value3 = DataRecordUtility.ExtractInt(table, record.value3, 0);
			attr4 = DataRecordUtility.ExtractInt(table, record.attr4, 0);
			value4 = DataRecordUtility.ExtractInt(table, record.value4, 0);
			attr5 = DataRecordUtility.ExtractInt(table, record.attr5, 0);
			value5 = DataRecordUtility.ExtractInt(table, record.value5, 0);
			attr6 = DataRecordUtility.ExtractInt(table, record.attr6, 0);
			value6 = DataRecordUtility.ExtractInt(table, record.value6, 0);
			attr7 = DataRecordUtility.ExtractInt(table, record.attr7, 0);
			value7 = DataRecordUtility.ExtractInt(table, record.value7, 0);
			attr8 = DataRecordUtility.ExtractInt(table, record.attr8, 0);
			value8 = DataRecordUtility.ExtractInt(table, record.value8, 0);
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			ImpactDataRecord record = new ImpactDataRecord();
			record.id = DataRecordUtility.SetValue(table, id, 0);
			record.desc = DataRecordUtility.SetValue(table, desc, "");
			record.type = DataRecordUtility.SetValue(table, type, 0);
			record.icon = DataRecordUtility.SetValue(table, icon, 0);
			record.duration = DataRecordUtility.SetValue(table, duration, 0);
			record.cooldown = DataRecordUtility.SetValue(table, cooldown, 0);
			record.multiple = DataRecordUtility.SetValue(table, multiple, null);
			record.damage = DataRecordUtility.SetValue(table, damage, null);
			record.vampire = DataRecordUtility.SetValue(table, vampire, null);
			record.attr1 = DataRecordUtility.SetValue(table, attr1, 0);
			record.value1 = DataRecordUtility.SetValue(table, value1, 0);
			record.attr2 = DataRecordUtility.SetValue(table, attr2, 0);
			record.value2 = DataRecordUtility.SetValue(table, value2, 0);
			record.attr3 = DataRecordUtility.SetValue(table, attr3, 0);
			record.value3 = DataRecordUtility.SetValue(table, value3, 0);
			record.attr4 = DataRecordUtility.SetValue(table, attr4, 0);
			record.value4 = DataRecordUtility.SetValue(table, value4, 0);
			record.attr5 = DataRecordUtility.SetValue(table, attr5, 0);
			record.value5 = DataRecordUtility.SetValue(table, value5, 0);
			record.attr6 = DataRecordUtility.SetValue(table, attr6, 0);
			record.value6 = DataRecordUtility.SetValue(table, value6, 0);
			record.attr7 = DataRecordUtility.SetValue(table, attr7, 0);
			record.value7 = DataRecordUtility.SetValue(table, value7, 0);
			record.attr8 = DataRecordUtility.SetValue(table, attr8, 0);
			record.value8 = DataRecordUtility.SetValue(table, value8, 0);
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		public int GetId()
		{
			return id;
		}

		private unsafe ImpactDataRecord GetRecord(BinaryTable table, int index)
		{
			ImpactDataRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(ImpactDataRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(ImpactDataRecord record)
		{
			byte[] bytes = new byte[sizeof(ImpactDataRecord)];
			fixed (byte* p = bytes) {
				ImpactDataRecord* temp = (ImpactDataRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class ImpactDataProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_ImpactData);
		}
		public void LoadForServer()
		{
			Load(FilePathDefine_Server.C_ImpactData);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_ImpactDataMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("ImpactData is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_ImpactDataMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_ImpactDataMgr.Clear();
		}

		public DataDictionaryMgr<ImpactData,int> ImpactDataMgr
		{
			get { return m_ImpactDataMgr; }
		}

		public int GetImpactDataCount()
		{
			return m_ImpactDataMgr.GetDataCount();
		}

		public ImpactData GetImpactData(int id)
		{
			return m_ImpactDataMgr.GetDataById(id);
		}

		private DataDictionaryMgr<ImpactData,int> m_ImpactDataMgr = new DataDictionaryMgr<ImpactData,int>();

		public static ImpactDataProvider Instance
		{
			get { return s_Instance; }
		}
		private static ImpactDataProvider s_Instance = new ImpactDataProvider();
	}
}

namespace TableConfig
{
	public sealed partial class Level : IDataRecord<int>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 64)]
		private struct LevelRecord
		{
			internal int id;
			internal int prefab;
			internal int type;
			internal int SceneDslFile;
			internal int ClientDslFile;
			internal int RoomDslFile;
			internal int ScenePlugins;
			internal int SceneUi;
			internal float EnterX;
			internal float EnterY;
			internal float EnterRadius;
			internal int RoomServer;
			internal int ThreadCountPerScene;
			internal int RoomCountPerThread;
			internal int MaxUserCount;
			internal int CanPK;
		}

		public int id;
		public string prefab;
		public int type;
		public List<string> SceneDslFile;
		public List<string> ClientDslFile;
		public List<string> RoomDslFile;
		public List<string> ScenePlugins;
		public List<int> SceneUi;
		public float EnterX;
		public float EnterY;
		public float EnterRadius;
		public List<string> RoomServer;
		public int ThreadCountPerScene;
		public int RoomCountPerThread;
		public int MaxUserCount;
		public bool CanPK;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			LevelRecord record = GetRecord(table,index);
			id = DataRecordUtility.ExtractInt(table, record.id, 0);
			prefab = DataRecordUtility.ExtractString(table, record.prefab, "");
			type = DataRecordUtility.ExtractInt(table, record.type, 0);
			SceneDslFile = DataRecordUtility.ExtractStringList(table, record.SceneDslFile, null);
			ClientDslFile = DataRecordUtility.ExtractStringList(table, record.ClientDslFile, null);
			RoomDslFile = DataRecordUtility.ExtractStringList(table, record.RoomDslFile, null);
			ScenePlugins = DataRecordUtility.ExtractStringList(table, record.ScenePlugins, null);
			SceneUi = DataRecordUtility.ExtractIntList(table, record.SceneUi, null);
			EnterX = DataRecordUtility.ExtractFloat(table, record.EnterX, 0);
			EnterY = DataRecordUtility.ExtractFloat(table, record.EnterY, 0);
			EnterRadius = DataRecordUtility.ExtractFloat(table, record.EnterRadius, 0);
			RoomServer = DataRecordUtility.ExtractStringList(table, record.RoomServer, null);
			ThreadCountPerScene = DataRecordUtility.ExtractInt(table, record.ThreadCountPerScene, 0);
			RoomCountPerThread = DataRecordUtility.ExtractInt(table, record.RoomCountPerThread, 0);
			MaxUserCount = DataRecordUtility.ExtractInt(table, record.MaxUserCount, 0);
			CanPK = DataRecordUtility.ExtractBool(table, record.CanPK, false);
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			LevelRecord record = new LevelRecord();
			record.id = DataRecordUtility.SetValue(table, id, 0);
			record.prefab = DataRecordUtility.SetValue(table, prefab, "");
			record.type = DataRecordUtility.SetValue(table, type, 0);
			record.SceneDslFile = DataRecordUtility.SetValue(table, SceneDslFile, null);
			record.ClientDslFile = DataRecordUtility.SetValue(table, ClientDslFile, null);
			record.RoomDslFile = DataRecordUtility.SetValue(table, RoomDslFile, null);
			record.ScenePlugins = DataRecordUtility.SetValue(table, ScenePlugins, null);
			record.SceneUi = DataRecordUtility.SetValue(table, SceneUi, null);
			record.EnterX = DataRecordUtility.SetValue(table, EnterX, 0);
			record.EnterY = DataRecordUtility.SetValue(table, EnterY, 0);
			record.EnterRadius = DataRecordUtility.SetValue(table, EnterRadius, 0);
			record.RoomServer = DataRecordUtility.SetValue(table, RoomServer, null);
			record.ThreadCountPerScene = DataRecordUtility.SetValue(table, ThreadCountPerScene, 0);
			record.RoomCountPerThread = DataRecordUtility.SetValue(table, RoomCountPerThread, 0);
			record.MaxUserCount = DataRecordUtility.SetValue(table, MaxUserCount, 0);
			record.CanPK = DataRecordUtility.SetValue(table, CanPK, false);
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		public int GetId()
		{
			return id;
		}

		private unsafe LevelRecord GetRecord(BinaryTable table, int index)
		{
			LevelRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(LevelRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(LevelRecord record)
		{
			byte[] bytes = new byte[sizeof(LevelRecord)];
			fixed (byte* p = bytes) {
				LevelRecord* temp = (LevelRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class LevelProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_Level);
		}
		public void LoadForServer()
		{
			Load(FilePathDefine_Server.C_Level);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_LevelMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("Level is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_LevelMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_LevelMgr.Clear();
		}

		public DataDictionaryMgr<Level,int> LevelMgr
		{
			get { return m_LevelMgr; }
		}

		public int GetLevelCount()
		{
			return m_LevelMgr.GetDataCount();
		}

		public Level GetLevel(int id)
		{
			return m_LevelMgr.GetDataById(id);
		}

		private DataDictionaryMgr<Level,int> m_LevelMgr = new DataDictionaryMgr<Level,int>();

		public static LevelProvider Instance
		{
			get { return s_Instance; }
		}
		private static LevelProvider s_Instance = new LevelProvider();
	}
}

namespace TableConfig
{
	public sealed partial class LevelMonster : IDataRecord
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 44)]
		private struct LevelMonsterRecord
		{
			internal int group;
			internal int scene;
			internal int camp;
			internal int actorID;
			internal float x;
			internal float y;
			internal float dir;
			internal int level;
			internal int passive;
			internal int aiLogic;
			internal int aiParams;
		}

		public int group;
		public int scene;
		public int camp;
		public int actorID;
		public float x;
		public float y;
		public float dir;
		public int level;
		public bool passive;
		public string aiLogic;
		public List<string> aiParams;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			LevelMonsterRecord record = GetRecord(table,index);
			group = DataRecordUtility.ExtractInt(table, record.group, 0);
			scene = DataRecordUtility.ExtractInt(table, record.scene, 0);
			camp = DataRecordUtility.ExtractInt(table, record.camp, 0);
			actorID = DataRecordUtility.ExtractInt(table, record.actorID, 0);
			x = DataRecordUtility.ExtractFloat(table, record.x, 0);
			y = DataRecordUtility.ExtractFloat(table, record.y, 0);
			dir = DataRecordUtility.ExtractFloat(table, record.dir, 0);
			level = DataRecordUtility.ExtractInt(table, record.level, 0);
			passive = DataRecordUtility.ExtractBool(table, record.passive, false);
			aiLogic = DataRecordUtility.ExtractString(table, record.aiLogic, "");
			aiParams = DataRecordUtility.ExtractStringList(table, record.aiParams, null);
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			LevelMonsterRecord record = new LevelMonsterRecord();
			record.group = DataRecordUtility.SetValue(table, group, 0);
			record.scene = DataRecordUtility.SetValue(table, scene, 0);
			record.camp = DataRecordUtility.SetValue(table, camp, 0);
			record.actorID = DataRecordUtility.SetValue(table, actorID, 0);
			record.x = DataRecordUtility.SetValue(table, x, 0);
			record.y = DataRecordUtility.SetValue(table, y, 0);
			record.dir = DataRecordUtility.SetValue(table, dir, 0);
			record.level = DataRecordUtility.SetValue(table, level, 0);
			record.passive = DataRecordUtility.SetValue(table, passive, false);
			record.aiLogic = DataRecordUtility.SetValue(table, aiLogic, "");
			record.aiParams = DataRecordUtility.SetValue(table, aiParams, null);
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		private unsafe LevelMonsterRecord GetRecord(BinaryTable table, int index)
		{
			LevelMonsterRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(LevelMonsterRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(LevelMonsterRecord record)
		{
			byte[] bytes = new byte[sizeof(LevelMonsterRecord)];
			fixed (byte* p = bytes) {
				LevelMonsterRecord* temp = (LevelMonsterRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class LevelMonsterProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_LevelMonster);
		}
		public void LoadForServer()
		{
			Load(FilePathDefine_Server.C_LevelMonster);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_LevelMonsterMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("LevelMonster is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_LevelMonsterMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_LevelMonsterMgr.Clear();
		}

		public DataListMgr<LevelMonster> LevelMonsterMgr
		{
			get { return m_LevelMonsterMgr; }
		}

		public int GetLevelMonsterCount()
		{
			return m_LevelMonsterMgr.GetDataCount();
		}

		private DataListMgr<LevelMonster> m_LevelMonsterMgr = new DataListMgr<LevelMonster>();

		public static LevelMonsterProvider Instance
		{
			get { return s_Instance; }
		}
		private static LevelMonsterProvider s_Instance = new LevelMonsterProvider();
	}
}

namespace TableConfig
{
	public sealed partial class Skill : IDataRecord<int>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 48)]
		private struct SkillRecord
		{
			internal int id;
			internal int desc;
			internal int type;
			internal int icon;
			internal int impacttoself;
			internal int impact;
			internal int targetType;
			internal int aoeType;
			internal float aoeSize;
			internal float aoeAngleOrLength;
			internal int maxAoeTargetCount;
			internal int dslFile;
		}

		public int id;
		public string desc;
		public int type;
		public int icon;
		public int impacttoself;
		public int impact;
		public int targetType;
		public int aoeType;
		public float aoeSize;
		public float aoeAngleOrLength;
		public int maxAoeTargetCount;
		public string dslFile;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			SkillRecord record = GetRecord(table,index);
			id = DataRecordUtility.ExtractInt(table, record.id, 0);
			desc = DataRecordUtility.ExtractString(table, record.desc, "");
			type = DataRecordUtility.ExtractInt(table, record.type, 0);
			icon = DataRecordUtility.ExtractInt(table, record.icon, 0);
			impacttoself = DataRecordUtility.ExtractInt(table, record.impacttoself, 0);
			impact = DataRecordUtility.ExtractInt(table, record.impact, 0);
			targetType = DataRecordUtility.ExtractInt(table, record.targetType, 0);
			aoeType = DataRecordUtility.ExtractInt(table, record.aoeType, 0);
			aoeSize = DataRecordUtility.ExtractFloat(table, record.aoeSize, 0);
			aoeAngleOrLength = DataRecordUtility.ExtractFloat(table, record.aoeAngleOrLength, 0);
			maxAoeTargetCount = DataRecordUtility.ExtractInt(table, record.maxAoeTargetCount, 0);
			dslFile = DataRecordUtility.ExtractString(table, record.dslFile, "");
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			SkillRecord record = new SkillRecord();
			record.id = DataRecordUtility.SetValue(table, id, 0);
			record.desc = DataRecordUtility.SetValue(table, desc, "");
			record.type = DataRecordUtility.SetValue(table, type, 0);
			record.icon = DataRecordUtility.SetValue(table, icon, 0);
			record.impacttoself = DataRecordUtility.SetValue(table, impacttoself, 0);
			record.impact = DataRecordUtility.SetValue(table, impact, 0);
			record.targetType = DataRecordUtility.SetValue(table, targetType, 0);
			record.aoeType = DataRecordUtility.SetValue(table, aoeType, 0);
			record.aoeSize = DataRecordUtility.SetValue(table, aoeSize, 0);
			record.aoeAngleOrLength = DataRecordUtility.SetValue(table, aoeAngleOrLength, 0);
			record.maxAoeTargetCount = DataRecordUtility.SetValue(table, maxAoeTargetCount, 0);
			record.dslFile = DataRecordUtility.SetValue(table, dslFile, "");
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

namespace TableConfig
{
	public sealed partial class SkillData : IDataRecord<int>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 140)]
		private struct SkillDataRecord
		{
			internal int id;
			internal int desc;
			internal int type;
			internal int icon;
			internal float distance;
			internal int cooldown;
			internal int canmove;
			internal int interruptPriority;
			internal int isInterrupt;
			internal int subsequentSkills;
			internal int autoCast;
			internal int needTarget;
			internal int multiple;
			internal int damage;
			internal int vampire;
			internal int addsc;
			internal int beaddsc;
			internal int adduc;
			internal int beadduc;
			internal int attr1;
			internal int value1;
			internal int attr2;
			internal int value2;
			internal int attr3;
			internal int value3;
			internal int attr4;
			internal int value4;
			internal int attr5;
			internal int value5;
			internal int attr6;
			internal int value6;
			internal int attr7;
			internal int value7;
			internal int attr8;
			internal int value8;
		}

		public int id;
		public string desc;
		public int type;
		public int icon;
		public float distance;
		public int cooldown;
		public int canmove;
		public int interruptPriority;
		public bool isInterrupt;
		public List<int> subsequentSkills;
		public int autoCast;
		public bool needTarget;
		public List<int> multiple;
		public List<int> damage;
		public List<int> vampire;
		public int addsc;
		public int beaddsc;
		public int adduc;
		public int beadduc;
		public int attr1;
		public int value1;
		public int attr2;
		public int value2;
		public int attr3;
		public int value3;
		public int attr4;
		public int value4;
		public int attr5;
		public int value5;
		public int attr6;
		public int value6;
		public int attr7;
		public int value7;
		public int attr8;
		public int value8;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			SkillDataRecord record = GetRecord(table,index);
			id = DataRecordUtility.ExtractInt(table, record.id, 0);
			desc = DataRecordUtility.ExtractString(table, record.desc, "");
			type = DataRecordUtility.ExtractInt(table, record.type, 0);
			icon = DataRecordUtility.ExtractInt(table, record.icon, 0);
			distance = DataRecordUtility.ExtractFloat(table, record.distance, 0);
			cooldown = DataRecordUtility.ExtractInt(table, record.cooldown, 0);
			canmove = DataRecordUtility.ExtractInt(table, record.canmove, 0);
			interruptPriority = DataRecordUtility.ExtractInt(table, record.interruptPriority, 0);
			isInterrupt = DataRecordUtility.ExtractBool(table, record.isInterrupt, false);
			subsequentSkills = DataRecordUtility.ExtractIntList(table, record.subsequentSkills, null);
			autoCast = DataRecordUtility.ExtractInt(table, record.autoCast, 0);
			needTarget = DataRecordUtility.ExtractBool(table, record.needTarget, false);
			multiple = DataRecordUtility.ExtractIntList(table, record.multiple, null);
			damage = DataRecordUtility.ExtractIntList(table, record.damage, null);
			vampire = DataRecordUtility.ExtractIntList(table, record.vampire, null);
			addsc = DataRecordUtility.ExtractInt(table, record.addsc, 0);
			beaddsc = DataRecordUtility.ExtractInt(table, record.beaddsc, 0);
			adduc = DataRecordUtility.ExtractInt(table, record.adduc, 0);
			beadduc = DataRecordUtility.ExtractInt(table, record.beadduc, 0);
			attr1 = DataRecordUtility.ExtractInt(table, record.attr1, 0);
			value1 = DataRecordUtility.ExtractInt(table, record.value1, 0);
			attr2 = DataRecordUtility.ExtractInt(table, record.attr2, 0);
			value2 = DataRecordUtility.ExtractInt(table, record.value2, 0);
			attr3 = DataRecordUtility.ExtractInt(table, record.attr3, 0);
			value3 = DataRecordUtility.ExtractInt(table, record.value3, 0);
			attr4 = DataRecordUtility.ExtractInt(table, record.attr4, 0);
			value4 = DataRecordUtility.ExtractInt(table, record.value4, 0);
			attr5 = DataRecordUtility.ExtractInt(table, record.attr5, 0);
			value5 = DataRecordUtility.ExtractInt(table, record.value5, 0);
			attr6 = DataRecordUtility.ExtractInt(table, record.attr6, 0);
			value6 = DataRecordUtility.ExtractInt(table, record.value6, 0);
			attr7 = DataRecordUtility.ExtractInt(table, record.attr7, 0);
			value7 = DataRecordUtility.ExtractInt(table, record.value7, 0);
			attr8 = DataRecordUtility.ExtractInt(table, record.attr8, 0);
			value8 = DataRecordUtility.ExtractInt(table, record.value8, 0);
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			SkillDataRecord record = new SkillDataRecord();
			record.id = DataRecordUtility.SetValue(table, id, 0);
			record.desc = DataRecordUtility.SetValue(table, desc, "");
			record.type = DataRecordUtility.SetValue(table, type, 0);
			record.icon = DataRecordUtility.SetValue(table, icon, 0);
			record.distance = DataRecordUtility.SetValue(table, distance, 0);
			record.cooldown = DataRecordUtility.SetValue(table, cooldown, 0);
			record.canmove = DataRecordUtility.SetValue(table, canmove, 0);
			record.interruptPriority = DataRecordUtility.SetValue(table, interruptPriority, 0);
			record.isInterrupt = DataRecordUtility.SetValue(table, isInterrupt, false);
			record.subsequentSkills = DataRecordUtility.SetValue(table, subsequentSkills, null);
			record.autoCast = DataRecordUtility.SetValue(table, autoCast, 0);
			record.needTarget = DataRecordUtility.SetValue(table, needTarget, false);
			record.multiple = DataRecordUtility.SetValue(table, multiple, null);
			record.damage = DataRecordUtility.SetValue(table, damage, null);
			record.vampire = DataRecordUtility.SetValue(table, vampire, null);
			record.addsc = DataRecordUtility.SetValue(table, addsc, 0);
			record.beaddsc = DataRecordUtility.SetValue(table, beaddsc, 0);
			record.adduc = DataRecordUtility.SetValue(table, adduc, 0);
			record.beadduc = DataRecordUtility.SetValue(table, beadduc, 0);
			record.attr1 = DataRecordUtility.SetValue(table, attr1, 0);
			record.value1 = DataRecordUtility.SetValue(table, value1, 0);
			record.attr2 = DataRecordUtility.SetValue(table, attr2, 0);
			record.value2 = DataRecordUtility.SetValue(table, value2, 0);
			record.attr3 = DataRecordUtility.SetValue(table, attr3, 0);
			record.value3 = DataRecordUtility.SetValue(table, value3, 0);
			record.attr4 = DataRecordUtility.SetValue(table, attr4, 0);
			record.value4 = DataRecordUtility.SetValue(table, value4, 0);
			record.attr5 = DataRecordUtility.SetValue(table, attr5, 0);
			record.value5 = DataRecordUtility.SetValue(table, value5, 0);
			record.attr6 = DataRecordUtility.SetValue(table, attr6, 0);
			record.value6 = DataRecordUtility.SetValue(table, value6, 0);
			record.attr7 = DataRecordUtility.SetValue(table, attr7, 0);
			record.value7 = DataRecordUtility.SetValue(table, value7, 0);
			record.attr8 = DataRecordUtility.SetValue(table, attr8, 0);
			record.value8 = DataRecordUtility.SetValue(table, value8, 0);
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		public int GetId()
		{
			return id;
		}

		private unsafe SkillDataRecord GetRecord(BinaryTable table, int index)
		{
			SkillDataRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(SkillDataRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(SkillDataRecord record)
		{
			byte[] bytes = new byte[sizeof(SkillDataRecord)];
			fixed (byte* p = bytes) {
				SkillDataRecord* temp = (SkillDataRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class SkillDataProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_SkillData);
		}
		public void LoadForServer()
		{
			Load(FilePathDefine_Server.C_SkillData);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_SkillDataMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("SkillData is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_SkillDataMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_SkillDataMgr.Clear();
		}

		public DataDictionaryMgr<SkillData,int> SkillDataMgr
		{
			get { return m_SkillDataMgr; }
		}

		public int GetSkillDataCount()
		{
			return m_SkillDataMgr.GetDataCount();
		}

		public SkillData GetSkillData(int id)
		{
			return m_SkillDataMgr.GetDataById(id);
		}

		private DataDictionaryMgr<SkillData,int> m_SkillDataMgr = new DataDictionaryMgr<SkillData,int>();

		public static SkillDataProvider Instance
		{
			get { return s_Instance; }
		}
		private static SkillDataProvider s_Instance = new SkillDataProvider();
	}
}

namespace TableConfig
{
	public sealed partial class SkillEvent : IDataRecord
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 92)]
		private struct SkillEventRecord
		{
			internal int actorId;
			internal int skillId;
			internal int eventId;
			internal int triggerBuffId;
			internal int triggerSkillId;
			internal int proc;
			internal int desc;
			internal int param1;
			internal int desc1;
			internal int param2;
			internal int desc2;
			internal int param3;
			internal int desc3;
			internal int param4;
			internal int desc4;
			internal int param5;
			internal int desc5;
			internal int param6;
			internal int desc6;
			internal int param7;
			internal int desc7;
			internal int param8;
			internal int desc8;
		}

		public int actorId;
		public int skillId;
		public int eventId;
		public int triggerBuffId;
		public int triggerSkillId;
		public string proc;
		public string desc;
		public int param1;
		public string desc1;
		public int param2;
		public string desc2;
		public int param3;
		public string desc3;
		public int param4;
		public string desc4;
		public int param5;
		public string desc5;
		public int param6;
		public string desc6;
		public int param7;
		public string desc7;
		public int param8;
		public string desc8;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			SkillEventRecord record = GetRecord(table,index);
			actorId = DataRecordUtility.ExtractInt(table, record.actorId, 0);
			skillId = DataRecordUtility.ExtractInt(table, record.skillId, 0);
			eventId = DataRecordUtility.ExtractInt(table, record.eventId, 0);
			triggerBuffId = DataRecordUtility.ExtractInt(table, record.triggerBuffId, 0);
			triggerSkillId = DataRecordUtility.ExtractInt(table, record.triggerSkillId, 0);
			proc = DataRecordUtility.ExtractString(table, record.proc, "");
			desc = DataRecordUtility.ExtractString(table, record.desc, "");
			param1 = DataRecordUtility.ExtractInt(table, record.param1, 0);
			desc1 = DataRecordUtility.ExtractString(table, record.desc1, "");
			param2 = DataRecordUtility.ExtractInt(table, record.param2, 0);
			desc2 = DataRecordUtility.ExtractString(table, record.desc2, "");
			param3 = DataRecordUtility.ExtractInt(table, record.param3, 0);
			desc3 = DataRecordUtility.ExtractString(table, record.desc3, "");
			param4 = DataRecordUtility.ExtractInt(table, record.param4, 0);
			desc4 = DataRecordUtility.ExtractString(table, record.desc4, "");
			param5 = DataRecordUtility.ExtractInt(table, record.param5, 0);
			desc5 = DataRecordUtility.ExtractString(table, record.desc5, "");
			param6 = DataRecordUtility.ExtractInt(table, record.param6, 0);
			desc6 = DataRecordUtility.ExtractString(table, record.desc6, "");
			param7 = DataRecordUtility.ExtractInt(table, record.param7, 0);
			desc7 = DataRecordUtility.ExtractString(table, record.desc7, "");
			param8 = DataRecordUtility.ExtractInt(table, record.param8, 0);
			desc8 = DataRecordUtility.ExtractString(table, record.desc8, "");
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			SkillEventRecord record = new SkillEventRecord();
			record.actorId = DataRecordUtility.SetValue(table, actorId, 0);
			record.skillId = DataRecordUtility.SetValue(table, skillId, 0);
			record.eventId = DataRecordUtility.SetValue(table, eventId, 0);
			record.triggerBuffId = DataRecordUtility.SetValue(table, triggerBuffId, 0);
			record.triggerSkillId = DataRecordUtility.SetValue(table, triggerSkillId, 0);
			record.proc = DataRecordUtility.SetValue(table, proc, "");
			record.desc = DataRecordUtility.SetValue(table, desc, "");
			record.param1 = DataRecordUtility.SetValue(table, param1, 0);
			record.desc1 = DataRecordUtility.SetValue(table, desc1, "");
			record.param2 = DataRecordUtility.SetValue(table, param2, 0);
			record.desc2 = DataRecordUtility.SetValue(table, desc2, "");
			record.param3 = DataRecordUtility.SetValue(table, param3, 0);
			record.desc3 = DataRecordUtility.SetValue(table, desc3, "");
			record.param4 = DataRecordUtility.SetValue(table, param4, 0);
			record.desc4 = DataRecordUtility.SetValue(table, desc4, "");
			record.param5 = DataRecordUtility.SetValue(table, param5, 0);
			record.desc5 = DataRecordUtility.SetValue(table, desc5, "");
			record.param6 = DataRecordUtility.SetValue(table, param6, 0);
			record.desc6 = DataRecordUtility.SetValue(table, desc6, "");
			record.param7 = DataRecordUtility.SetValue(table, param7, 0);
			record.desc7 = DataRecordUtility.SetValue(table, desc7, "");
			record.param8 = DataRecordUtility.SetValue(table, param8, 0);
			record.desc8 = DataRecordUtility.SetValue(table, desc8, "");
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		private unsafe SkillEventRecord GetRecord(BinaryTable table, int index)
		{
			SkillEventRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(SkillEventRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(SkillEventRecord record)
		{
			byte[] bytes = new byte[sizeof(SkillEventRecord)];
			fixed (byte* p = bytes) {
				SkillEventRecord* temp = (SkillEventRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class SkillEventProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_SkillEvent);
		}
		public void LoadForServer()
		{
			Load(FilePathDefine_Server.C_SkillEvent);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_SkillEventMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("SkillEvent is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_SkillEventMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_SkillEventMgr.Clear();
		}

		public DataListMgr<SkillEvent> SkillEventMgr
		{
			get { return m_SkillEventMgr; }
		}

		public int GetSkillEventCount()
		{
			return m_SkillEventMgr.GetDataCount();
		}

		private DataListMgr<SkillEvent> m_SkillEventMgr = new DataListMgr<SkillEvent>();

		public static SkillEventProvider Instance
		{
			get { return s_Instance; }
		}
		private static SkillEventProvider s_Instance = new SkillEventProvider();
	}
}

namespace TableConfig
{
	public sealed partial class SkillResources : IDataRecord
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 12)]
		private struct SkillResourcesRecord
		{
			internal int skillId;
			internal int key;
			internal int resource;
		}

		public int skillId;
		public string key;
		public string resource;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			SkillResourcesRecord record = GetRecord(table,index);
			skillId = DataRecordUtility.ExtractInt(table, record.skillId, 0);
			key = DataRecordUtility.ExtractString(table, record.key, "");
			resource = DataRecordUtility.ExtractString(table, record.resource, "");
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			SkillResourcesRecord record = new SkillResourcesRecord();
			record.skillId = DataRecordUtility.SetValue(table, skillId, 0);
			record.key = DataRecordUtility.SetValue(table, key, "");
			record.resource = DataRecordUtility.SetValue(table, resource, "");
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		private unsafe SkillResourcesRecord GetRecord(BinaryTable table, int index)
		{
			SkillResourcesRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(SkillResourcesRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(SkillResourcesRecord record)
		{
			byte[] bytes = new byte[sizeof(SkillResourcesRecord)];
			fixed (byte* p = bytes) {
				SkillResourcesRecord* temp = (SkillResourcesRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class SkillResourcesProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_SkillResources);
		}
		public void LoadForServer()
		{
			Load(FilePathDefine_Server.C_SkillResources);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_SkillResourcesMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("SkillResources is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_SkillResourcesMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_SkillResourcesMgr.Clear();
		}

		public DataListMgr<SkillResources> SkillResourcesMgr
		{
			get { return m_SkillResourcesMgr; }
		}

		public int GetSkillResourcesCount()
		{
			return m_SkillResourcesMgr.GetDataCount();
		}

		private DataListMgr<SkillResources> m_SkillResourcesMgr = new DataListMgr<SkillResources>();

		public static SkillResourcesProvider Instance
		{
			get { return s_Instance; }
		}
		private static SkillResourcesProvider s_Instance = new SkillResourcesProvider();
	}
}

namespace TableConfig
{
	public sealed partial class StoryDlg : IDataRecord<int>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 24)]
		private struct StoryDlgRecord
		{
			internal int id;
			internal int dialogId;
			internal int index;
			internal int speaker;
			internal int leftOrRight;
			internal int dialog;
		}

		public int id;
		public int dialogId;
		public int index;
		public int speaker;
		public int leftOrRight;
		public string dialog;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			StoryDlgRecord record = GetRecord(table,index);
			id = DataRecordUtility.ExtractInt(table, record.id, 0);
			dialogId = DataRecordUtility.ExtractInt(table, record.dialogId, 0);
			index = DataRecordUtility.ExtractInt(table, record.index, 0);
			speaker = DataRecordUtility.ExtractInt(table, record.speaker, 0);
			leftOrRight = DataRecordUtility.ExtractInt(table, record.leftOrRight, 0);
			dialog = DataRecordUtility.ExtractString(table, record.dialog, "");
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			StoryDlgRecord record = new StoryDlgRecord();
			record.id = DataRecordUtility.SetValue(table, id, 0);
			record.dialogId = DataRecordUtility.SetValue(table, dialogId, 0);
			record.index = DataRecordUtility.SetValue(table, index, 0);
			record.speaker = DataRecordUtility.SetValue(table, speaker, 0);
			record.leftOrRight = DataRecordUtility.SetValue(table, leftOrRight, 0);
			record.dialog = DataRecordUtility.SetValue(table, dialog, "");
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		public int GetId()
		{
			return id;
		}

		private unsafe StoryDlgRecord GetRecord(BinaryTable table, int index)
		{
			StoryDlgRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(StoryDlgRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(StoryDlgRecord record)
		{
			byte[] bytes = new byte[sizeof(StoryDlgRecord)];
			fixed (byte* p = bytes) {
				StoryDlgRecord* temp = (StoryDlgRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class StoryDlgProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_StoryDlg);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_StoryDlgMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("StoryDlg is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_StoryDlgMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_StoryDlgMgr.Clear();
		}

		public DataDictionaryMgr<StoryDlg,int> StoryDlgMgr
		{
			get { return m_StoryDlgMgr; }
		}

		public int GetStoryDlgCount()
		{
			return m_StoryDlgMgr.GetDataCount();
		}

		public StoryDlg GetStoryDlg(int id)
		{
			return m_StoryDlgMgr.GetDataById(id);
		}

		private DataDictionaryMgr<StoryDlg,int> m_StoryDlgMgr = new DataDictionaryMgr<StoryDlg,int>();

		public static StoryDlgProvider Instance
		{
			get { return s_Instance; }
		}
		private static StoryDlgProvider s_Instance = new StoryDlgProvider();
	}
}

namespace TableConfig
{
	public sealed partial class StrDictionary : IDataRecord<string>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 8)]
		private struct StrDictionaryRecord
		{
			internal int id;
			internal int Content;
		}

		public string id;
		public string Content;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			StrDictionaryRecord record = GetRecord(table,index);
			id = DataRecordUtility.ExtractString(table, record.id, "");
			Content = DataRecordUtility.ExtractString(table, record.Content, "");
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			StrDictionaryRecord record = new StrDictionaryRecord();
			record.id = DataRecordUtility.SetValue(table, id, "");
			record.Content = DataRecordUtility.SetValue(table, Content, "");
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		public string GetId()
		{
			return id;
		}

		private unsafe StrDictionaryRecord GetRecord(BinaryTable table, int index)
		{
			StrDictionaryRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(StrDictionaryRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(StrDictionaryRecord record)
		{
			byte[] bytes = new byte[sizeof(StrDictionaryRecord)];
			fixed (byte* p = bytes) {
				StrDictionaryRecord* temp = (StrDictionaryRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class StrDictionaryProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_StrDictionary);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_StrDictionaryMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("StrDictionary is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_StrDictionaryMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_StrDictionaryMgr.Clear();
		}

		public DataDictionaryMgr<StrDictionary,string> StrDictionaryMgr
		{
			get { return m_StrDictionaryMgr; }
		}

		public int GetStrDictionaryCount()
		{
			return m_StrDictionaryMgr.GetDataCount();
		}

		public StrDictionary GetStrDictionary(string id)
		{
			return m_StrDictionaryMgr.GetDataById(id);
		}

		private DataDictionaryMgr<StrDictionary,string> m_StrDictionaryMgr = new DataDictionaryMgr<StrDictionary,string>();

		public static StrDictionaryProvider Instance
		{
			get { return s_Instance; }
		}
		private static StrDictionaryProvider s_Instance = new StrDictionaryProvider();
	}
}

namespace TableConfig
{
	public sealed partial class UI : IDataRecord<int>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 20)]
		private struct UIRecord
		{
			internal int id;
			internal int name;
			internal int path;
			internal int dsl;
			internal int visible;
		}

		public int id;
		public string name;
		public string path;
		public string dsl;
		public bool visible;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			UIRecord record = GetRecord(table,index);
			id = DataRecordUtility.ExtractInt(table, record.id, 0);
			name = DataRecordUtility.ExtractString(table, record.name, "");
			path = DataRecordUtility.ExtractString(table, record.path, "");
			dsl = DataRecordUtility.ExtractString(table, record.dsl, "");
			visible = DataRecordUtility.ExtractBool(table, record.visible, false);
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			UIRecord record = new UIRecord();
			record.id = DataRecordUtility.SetValue(table, id, 0);
			record.name = DataRecordUtility.SetValue(table, name, "");
			record.path = DataRecordUtility.SetValue(table, path, "");
			record.dsl = DataRecordUtility.SetValue(table, dsl, "");
			record.visible = DataRecordUtility.SetValue(table, visible, false);
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		public int GetId()
		{
			return id;
		}

		private unsafe UIRecord GetRecord(BinaryTable table, int index)
		{
			UIRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(UIRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(UIRecord record)
		{
			byte[] bytes = new byte[sizeof(UIRecord)];
			fixed (byte* p = bytes) {
				UIRecord* temp = (UIRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class UIProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_UI);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_UIMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("UI is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_UIMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_UIMgr.Clear();
		}

		public DataDictionaryMgr<UI,int> UIMgr
		{
			get { return m_UIMgr; }
		}

		public int GetUICount()
		{
			return m_UIMgr.GetDataCount();
		}

		public UI GetUI(int id)
		{
			return m_UIMgr.GetDataById(id);
		}

		private DataDictionaryMgr<UI,int> m_UIMgr = new DataDictionaryMgr<UI,int>();

		public static UIProvider Instance
		{
			get { return s_Instance; }
		}
		private static UIProvider s_Instance = new UIProvider();
	}
}

namespace TableConfig
{
	public sealed partial class UserScript : IDataRecord<string>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 16)]
		private struct UserScriptRecord
		{
			internal int id;
			internal int StoryId;
			internal int Namespace;
			internal int DslFile;
		}

		public string id;
		public string StoryId;
		public string Namespace;
		public string DslFile;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			UserScriptRecord record = GetRecord(table,index);
			id = DataRecordUtility.ExtractString(table, record.id, "");
			StoryId = DataRecordUtility.ExtractString(table, record.StoryId, "");
			Namespace = DataRecordUtility.ExtractString(table, record.Namespace, "");
			DslFile = DataRecordUtility.ExtractString(table, record.DslFile, "");
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			UserScriptRecord record = new UserScriptRecord();
			record.id = DataRecordUtility.SetValue(table, id, "");
			record.StoryId = DataRecordUtility.SetValue(table, StoryId, "");
			record.Namespace = DataRecordUtility.SetValue(table, Namespace, "");
			record.DslFile = DataRecordUtility.SetValue(table, DslFile, "");
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		public string GetId()
		{
			return id;
		}

		private unsafe UserScriptRecord GetRecord(BinaryTable table, int index)
		{
			UserScriptRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(UserScriptRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(UserScriptRecord record)
		{
			byte[] bytes = new byte[sizeof(UserScriptRecord)];
			fixed (byte* p = bytes) {
				UserScriptRecord* temp = (UserScriptRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class UserScriptProvider
	{
		public void LoadForServer()
		{
			Load(FilePathDefine_Server.C_UserScript);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_UserScriptMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("UserScript is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_UserScriptMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_UserScriptMgr.Clear();
		}

		public DataDictionaryMgr<UserScript,string> UserScriptMgr
		{
			get { return m_UserScriptMgr; }
		}

		public int GetUserScriptCount()
		{
			return m_UserScriptMgr.GetDataCount();
		}

		public UserScript GetUserScript(string id)
		{
			return m_UserScriptMgr.GetDataById(id);
		}

		private DataDictionaryMgr<UserScript,string> m_UserScriptMgr = new DataDictionaryMgr<UserScript,string>();

		public static UserScriptProvider Instance
		{
			get { return s_Instance; }
		}
		private static UserScriptProvider s_Instance = new UserScriptProvider();
	}
}
