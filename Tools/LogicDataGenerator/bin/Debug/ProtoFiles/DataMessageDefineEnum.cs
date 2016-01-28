//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/DataMessageDefine.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace GameFrameworkMessage
{
	public enum DataMessageEnum
	{
		Msg_LD_Connect = 1,
		Msg_DL_Connect,
		Msg_LD_Load,
		Msg_DL_LoadResult,
		Msg_LD_Save,
		Msg_DL_SaveResult,
	}

	public static class DataMessageEnum2Type
	{
		public static Type Query(int id)
		{
			Type t;
			s_DataMessageEnum2Type.TryGetValue(id, out t);
			return t;
		}

		public static int Query(Type t)
		{
			int id;
			s_Type2DataMessageEnum.TryGetValue(t, out id);
			return id;
		}

		static DataMessageEnum2Type()
		{
			s_DataMessageEnum2Type.Add((int)DataMessageEnum.Msg_DL_Connect, typeof(Msg_DL_Connect));
			s_Type2DataMessageEnum.Add(typeof(Msg_DL_Connect), (int)DataMessageEnum.Msg_DL_Connect);
			s_DataMessageEnum2Type.Add((int)DataMessageEnum.Msg_DL_LoadResult, typeof(Msg_DL_LoadResult));
			s_Type2DataMessageEnum.Add(typeof(Msg_DL_LoadResult), (int)DataMessageEnum.Msg_DL_LoadResult);
			s_DataMessageEnum2Type.Add((int)DataMessageEnum.Msg_DL_SaveResult, typeof(Msg_DL_SaveResult));
			s_Type2DataMessageEnum.Add(typeof(Msg_DL_SaveResult), (int)DataMessageEnum.Msg_DL_SaveResult);
			s_DataMessageEnum2Type.Add((int)DataMessageEnum.Msg_LD_Connect, typeof(Msg_LD_Connect));
			s_Type2DataMessageEnum.Add(typeof(Msg_LD_Connect), (int)DataMessageEnum.Msg_LD_Connect);
			s_DataMessageEnum2Type.Add((int)DataMessageEnum.Msg_LD_Load, typeof(Msg_LD_Load));
			s_Type2DataMessageEnum.Add(typeof(Msg_LD_Load), (int)DataMessageEnum.Msg_LD_Load);
			s_DataMessageEnum2Type.Add((int)DataMessageEnum.Msg_LD_Save, typeof(Msg_LD_Save));
			s_Type2DataMessageEnum.Add(typeof(Msg_LD_Save), (int)DataMessageEnum.Msg_LD_Save);
		}

		private static Dictionary<int, Type> s_DataMessageEnum2Type = new Dictionary<int, Type>();
		private static Dictionary<Type, int> s_Type2DataMessageEnum = new Dictionary<Type, int>();
	}
}

