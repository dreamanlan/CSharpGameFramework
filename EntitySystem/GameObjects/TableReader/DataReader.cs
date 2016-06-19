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
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 120)]
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
			internal int hp;
			internal int mp;
			internal int attack;
			internal int defence;
			internal int addhp;
			internal int addmp;
			internal int addattack;
			internal int adddefence;
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
		public int hp;
		public int mp;
		public int attack;
		public int defence;
		public int addhp;
		public int addmp;
		public int addattack;
		public int adddefence;

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
			hp = DataRecordUtility.ExtractInt(table, record.hp, 0);
			mp = DataRecordUtility.ExtractInt(table, record.mp, 0);
			attack = DataRecordUtility.ExtractInt(table, record.attack, 0);
			defence = DataRecordUtility.ExtractInt(table, record.defence, 0);
			addhp = DataRecordUtility.ExtractInt(table, record.addhp, 0);
			addmp = DataRecordUtility.ExtractInt(table, record.addmp, 0);
			addattack = DataRecordUtility.ExtractInt(table, record.addattack, 0);
			adddefence = DataRecordUtility.ExtractInt(table, record.adddefence, 0);
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
			record.hp = DataRecordUtility.SetValue(table, hp, 0);
			record.mp = DataRecordUtility.SetValue(table, mp, 0);
			record.attack = DataRecordUtility.SetValue(table, attack, 0);
			record.defence = DataRecordUtility.SetValue(table, defence, 0);
			record.addhp = DataRecordUtility.SetValue(table, addhp, 0);
			record.addmp = DataRecordUtility.SetValue(table, addmp, 0);
			record.addattack = DataRecordUtility.SetValue(table, addattack, 0);
			record.adddefence = DataRecordUtility.SetValue(table, adddefence, 0);
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
	public sealed partial class Level : IDataRecord<int>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 60)]
		private struct LevelRecord
		{
			internal int id;
			internal int prefab;
			internal int type;
			internal int SceneDslFile;
			internal int ClientDslFile;
			internal int RoomDslFile;
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
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 36)]
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
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 136)]
		private struct SkillRecord
		{
			internal int id;
			internal int desc;
			internal int type;
			internal int icon;
			internal float distance;
			internal int duration;
			internal int interval;
			internal int canmove;
			internal int impact;
			internal int interruptPriority;
			internal int isInterrupt;
			internal int targetType;
			internal int aoeType;
			internal float aoeSize;
			internal float aoeAngleOrLength;
			internal int maxAoeTargetCount;
			internal int dslSkillId;
			internal int startupSkillId;
			internal int flybackSkillId;
			internal int startupPositionType;
			internal int subsequentSkills;
			internal int autoCast;
			internal int needTarget;
			internal int cooldown;
			internal int damage;
			internal int addhp;
			internal int addmp;
			internal int addattack;
			internal int adddefence;
			internal int addshield;
			internal int addspeed;
			internal int addcritical;
			internal int addcriticalpow;
			internal int addrps;
		}

		public int id;
		public string desc;
		public int type;
		public int icon;
		public float distance;
		public int duration;
		public int interval;
		public int canmove;
		public int impact;
		public int interruptPriority;
		public bool isInterrupt;
		public int targetType;
		public int aoeType;
		public float aoeSize;
		public float aoeAngleOrLength;
		public int maxAoeTargetCount;
		public int dslSkillId;
		public int startupSkillId;
		public int flybackSkillId;
		public int startupPositionType;
		public List<int> subsequentSkills;
		public int autoCast;
		public bool needTarget;
		public int cooldown;
		public int damage;
		public int addhp;
		public int addmp;
		public int addattack;
		public int adddefence;
		public int addshield;
		public int addspeed;
		public int addcritical;
		public int addcriticalpow;
		public int addrps;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			SkillRecord record = GetRecord(table,index);
			id = DataRecordUtility.ExtractInt(table, record.id, 0);
			desc = DataRecordUtility.ExtractString(table, record.desc, "");
			type = DataRecordUtility.ExtractInt(table, record.type, 0);
			icon = DataRecordUtility.ExtractInt(table, record.icon, 0);
			distance = DataRecordUtility.ExtractFloat(table, record.distance, 0);
			duration = DataRecordUtility.ExtractInt(table, record.duration, 0);
			interval = DataRecordUtility.ExtractInt(table, record.interval, 0);
			canmove = DataRecordUtility.ExtractInt(table, record.canmove, 0);
			impact = DataRecordUtility.ExtractInt(table, record.impact, 0);
			interruptPriority = DataRecordUtility.ExtractInt(table, record.interruptPriority, 0);
			isInterrupt = DataRecordUtility.ExtractBool(table, record.isInterrupt, false);
			targetType = DataRecordUtility.ExtractInt(table, record.targetType, 0);
			aoeType = DataRecordUtility.ExtractInt(table, record.aoeType, 0);
			aoeSize = DataRecordUtility.ExtractFloat(table, record.aoeSize, 0);
			aoeAngleOrLength = DataRecordUtility.ExtractFloat(table, record.aoeAngleOrLength, 0);
			maxAoeTargetCount = DataRecordUtility.ExtractInt(table, record.maxAoeTargetCount, 0);
			dslSkillId = DataRecordUtility.ExtractInt(table, record.dslSkillId, 0);
			startupSkillId = DataRecordUtility.ExtractInt(table, record.startupSkillId, 0);
			flybackSkillId = DataRecordUtility.ExtractInt(table, record.flybackSkillId, 0);
			startupPositionType = DataRecordUtility.ExtractInt(table, record.startupPositionType, 0);
			subsequentSkills = DataRecordUtility.ExtractIntList(table, record.subsequentSkills, null);
			autoCast = DataRecordUtility.ExtractInt(table, record.autoCast, 0);
			needTarget = DataRecordUtility.ExtractBool(table, record.needTarget, false);
			cooldown = DataRecordUtility.ExtractInt(table, record.cooldown, 0);
			damage = DataRecordUtility.ExtractInt(table, record.damage, 0);
			addhp = DataRecordUtility.ExtractInt(table, record.addhp, 0);
			addmp = DataRecordUtility.ExtractInt(table, record.addmp, 0);
			addattack = DataRecordUtility.ExtractInt(table, record.addattack, 0);
			adddefence = DataRecordUtility.ExtractInt(table, record.adddefence, 0);
			addshield = DataRecordUtility.ExtractInt(table, record.addshield, 0);
			addspeed = DataRecordUtility.ExtractInt(table, record.addspeed, 0);
			addcritical = DataRecordUtility.ExtractInt(table, record.addcritical, 0);
			addcriticalpow = DataRecordUtility.ExtractInt(table, record.addcriticalpow, 0);
			addrps = DataRecordUtility.ExtractInt(table, record.addrps, 0);
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
			record.duration = DataRecordUtility.SetValue(table, duration, 0);
			record.interval = DataRecordUtility.SetValue(table, interval, 0);
			record.canmove = DataRecordUtility.SetValue(table, canmove, 0);
			record.impact = DataRecordUtility.SetValue(table, impact, 0);
			record.interruptPriority = DataRecordUtility.SetValue(table, interruptPriority, 0);
			record.isInterrupt = DataRecordUtility.SetValue(table, isInterrupt, false);
			record.targetType = DataRecordUtility.SetValue(table, targetType, 0);
			record.aoeType = DataRecordUtility.SetValue(table, aoeType, 0);
			record.aoeSize = DataRecordUtility.SetValue(table, aoeSize, 0);
			record.aoeAngleOrLength = DataRecordUtility.SetValue(table, aoeAngleOrLength, 0);
			record.maxAoeTargetCount = DataRecordUtility.SetValue(table, maxAoeTargetCount, 0);
			record.dslSkillId = DataRecordUtility.SetValue(table, dslSkillId, 0);
			record.startupSkillId = DataRecordUtility.SetValue(table, startupSkillId, 0);
			record.flybackSkillId = DataRecordUtility.SetValue(table, flybackSkillId, 0);
			record.startupPositionType = DataRecordUtility.SetValue(table, startupPositionType, 0);
			record.subsequentSkills = DataRecordUtility.SetValue(table, subsequentSkills, null);
			record.autoCast = DataRecordUtility.SetValue(table, autoCast, 0);
			record.needTarget = DataRecordUtility.SetValue(table, needTarget, false);
			record.cooldown = DataRecordUtility.SetValue(table, cooldown, 0);
			record.damage = DataRecordUtility.SetValue(table, damage, 0);
			record.addhp = DataRecordUtility.SetValue(table, addhp, 0);
			record.addmp = DataRecordUtility.SetValue(table, addmp, 0);
			record.addattack = DataRecordUtility.SetValue(table, addattack, 0);
			record.adddefence = DataRecordUtility.SetValue(table, adddefence, 0);
			record.addshield = DataRecordUtility.SetValue(table, addshield, 0);
			record.addspeed = DataRecordUtility.SetValue(table, addspeed, 0);
			record.addcritical = DataRecordUtility.SetValue(table, addcritical, 0);
			record.addcriticalpow = DataRecordUtility.SetValue(table, addcriticalpow, 0);
			record.addrps = DataRecordUtility.SetValue(table, addrps, 0);
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
	public sealed partial class SkillDsl : IDataRecord<int>
	{
		[StructLayout(LayoutKind.Auto, Pack = 1, Size = 8)]
		private struct SkillDslRecord
		{
			internal int id;
			internal int dslFile;
		}

		public int id;
		public string dslFile;

		public bool ReadFromBinary(BinaryTable table, int index)
		{
			SkillDslRecord record = GetRecord(table,index);
			id = DataRecordUtility.ExtractInt(table, record.id, 0);
			dslFile = DataRecordUtility.ExtractString(table, record.dslFile, "");
			return true;
		}

		public void WriteToBinary(BinaryTable table)
		{
			SkillDslRecord record = new SkillDslRecord();
			record.id = DataRecordUtility.SetValue(table, id, 0);
			record.dslFile = DataRecordUtility.SetValue(table, dslFile, "");
			byte[] bytes = GetRecordBytes(record);
			table.Records.Add(bytes);
		}

		public int GetId()
		{
			return id;
		}

		private unsafe SkillDslRecord GetRecord(BinaryTable table, int index)
		{
			SkillDslRecord record;
			byte[] bytes = table.Records[index];
			fixed (byte* p = bytes) {
				record = *(SkillDslRecord*)p;
			}
			return record;
		}
		private static unsafe byte[] GetRecordBytes(SkillDslRecord record)
		{
			byte[] bytes = new byte[sizeof(SkillDslRecord)];
			fixed (byte* p = bytes) {
				SkillDslRecord* temp = (SkillDslRecord*)p;
				*temp = record;
			}
			return bytes;
		}
	}

	public sealed partial class SkillDslProvider
	{
		public void LoadForClient()
		{
			Load(FilePathDefine_Client.C_SkillDsl);
		}
		public void LoadForServer()
		{
			Load(FilePathDefine_Server.C_SkillDsl);
		}
		public void Load(string file)
		{
			if (BinaryTable.IsValid(HomePath.GetAbsolutePath(file))) {
				m_SkillDslMgr.LoadFromBinary(file);
			} else {
				LogSystem.Error("SkillDsl is not a table !");
			}
		}
		public void Save(string file)
		{
		#if DEBUG
			m_SkillDslMgr.SaveToBinary(file);
		#endif
		}
		public void Clear()
		{
			m_SkillDslMgr.Clear();
		}

		public DataDictionaryMgr<SkillDsl,int> SkillDslMgr
		{
			get { return m_SkillDslMgr; }
		}

		public int GetSkillDslCount()
		{
			return m_SkillDslMgr.GetDataCount();
		}

		public SkillDsl GetSkillDsl(int id)
		{
			return m_SkillDslMgr.GetDataById(id);
		}

		private DataDictionaryMgr<SkillDsl,int> m_SkillDslMgr = new DataDictionaryMgr<SkillDsl,int>();

		public static SkillDslProvider Instance
		{
			get { return s_Instance; }
		}
		private static SkillDslProvider s_Instance = new SkillDslProvider();
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
