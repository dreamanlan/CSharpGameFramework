
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