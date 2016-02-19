//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按DataProto/Data.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace GameFrameworkData
{
	public enum DataEnum
	{
		TableGlobalParam = 1,
		TableGuid,
		TableNicknameInfo,
		TableMailInfo,
		TableActivationCode,
		TableAccount,
		TableUserInfo,
		TableMemberInfo,
		TableItemInfo,
		TableMailStateInfo,
		TableFriendInfo,
		MaxNum
	}

	public static class DataEnum2Type
	{
		public static Type Query(int id)
		{
			Type t;
			s_DataEnum2Type.TryGetValue(id, out t);
			return t;
		}

		public static int Query(Type t)
		{
			int id;
			s_Type2DataEnum.TryGetValue(t, out id);
			return id;
		}

		static DataEnum2Type()
		{
			s_DataEnum2Type.Add((int)DataEnum.TableAccount, typeof(TableAccount));
			s_Type2DataEnum.Add(typeof(TableAccount), (int)DataEnum.TableAccount);
			s_DataEnum2Type.Add((int)DataEnum.TableActivationCode, typeof(TableActivationCode));
			s_Type2DataEnum.Add(typeof(TableActivationCode), (int)DataEnum.TableActivationCode);
			s_DataEnum2Type.Add((int)DataEnum.TableFriendInfo, typeof(TableFriendInfo));
			s_Type2DataEnum.Add(typeof(TableFriendInfo), (int)DataEnum.TableFriendInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableGlobalParam, typeof(TableGlobalParam));
			s_Type2DataEnum.Add(typeof(TableGlobalParam), (int)DataEnum.TableGlobalParam);
			s_DataEnum2Type.Add((int)DataEnum.TableGuid, typeof(TableGuid));
			s_Type2DataEnum.Add(typeof(TableGuid), (int)DataEnum.TableGuid);
			s_DataEnum2Type.Add((int)DataEnum.TableItemInfo, typeof(TableItemInfo));
			s_Type2DataEnum.Add(typeof(TableItemInfo), (int)DataEnum.TableItemInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableMailInfo, typeof(TableMailInfo));
			s_Type2DataEnum.Add(typeof(TableMailInfo), (int)DataEnum.TableMailInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableMailStateInfo, typeof(TableMailStateInfo));
			s_Type2DataEnum.Add(typeof(TableMailStateInfo), (int)DataEnum.TableMailStateInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableMemberInfo, typeof(TableMemberInfo));
			s_Type2DataEnum.Add(typeof(TableMemberInfo), (int)DataEnum.TableMemberInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableNicknameInfo, typeof(TableNicknameInfo));
			s_Type2DataEnum.Add(typeof(TableNicknameInfo), (int)DataEnum.TableNicknameInfo);
			s_DataEnum2Type.Add((int)DataEnum.TableUserInfo, typeof(TableUserInfo));
			s_Type2DataEnum.Add(typeof(TableUserInfo), (int)DataEnum.TableUserInfo);
		}

		private static Dictionary<int, Type> s_DataEnum2Type = new Dictionary<int, Type>();
		private static Dictionary<Type, int> s_Type2DataEnum = new Dictionary<Type, int>();
	}
}

