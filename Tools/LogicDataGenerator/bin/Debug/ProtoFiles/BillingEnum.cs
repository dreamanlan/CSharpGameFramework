//----------------------------------------------------------------------------
//！！！不要手动修改此文件，此文件由LogicDataGenerator按ProtoFiles/Billing.dsl生成！！！
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace GameFrameworkMessage
{
	public enum BillingMessageEnum
	{
		LB_VerifyAccount = 1,
		BL_VerifyAccountResult,
		LB_VerifyCode,
		BL_VerifyCodeResult,
		LB_UseCode,
		BL_UseCodeResult,
		BL_PayOrder,
		LB_PayOrderResult,
	}

	public static class BillingMessageEnum2Type
	{
		public static Type Query(int id)
		{
			Type t;
			s_BillingMessageEnum2Type.TryGetValue(id, out t);
			return t;
		}

		public static int Query(Type t)
		{
			int id;
			s_Type2BillingMessageEnum.TryGetValue(t, out id);
			return id;
		}

		static BillingMessageEnum2Type()
		{
			s_BillingMessageEnum2Type.Add((int)BillingMessageEnum.BL_PayOrder, typeof(BL_PayOrder));
			s_Type2BillingMessageEnum.Add(typeof(BL_PayOrder), (int)BillingMessageEnum.BL_PayOrder);
			s_BillingMessageEnum2Type.Add((int)BillingMessageEnum.BL_UseCodeResult, typeof(BL_UseCodeResult));
			s_Type2BillingMessageEnum.Add(typeof(BL_UseCodeResult), (int)BillingMessageEnum.BL_UseCodeResult);
			s_BillingMessageEnum2Type.Add((int)BillingMessageEnum.BL_VerifyAccountResult, typeof(BL_VerifyAccountResult));
			s_Type2BillingMessageEnum.Add(typeof(BL_VerifyAccountResult), (int)BillingMessageEnum.BL_VerifyAccountResult);
			s_BillingMessageEnum2Type.Add((int)BillingMessageEnum.BL_VerifyCodeResult, typeof(BL_VerifyCodeResult));
			s_Type2BillingMessageEnum.Add(typeof(BL_VerifyCodeResult), (int)BillingMessageEnum.BL_VerifyCodeResult);
			s_BillingMessageEnum2Type.Add((int)BillingMessageEnum.LB_PayOrderResult, typeof(LB_PayOrderResult));
			s_Type2BillingMessageEnum.Add(typeof(LB_PayOrderResult), (int)BillingMessageEnum.LB_PayOrderResult);
			s_BillingMessageEnum2Type.Add((int)BillingMessageEnum.LB_UseCode, typeof(LB_UseCode));
			s_Type2BillingMessageEnum.Add(typeof(LB_UseCode), (int)BillingMessageEnum.LB_UseCode);
			s_BillingMessageEnum2Type.Add((int)BillingMessageEnum.LB_VerifyAccount, typeof(LB_VerifyAccount));
			s_Type2BillingMessageEnum.Add(typeof(LB_VerifyAccount), (int)BillingMessageEnum.LB_VerifyAccount);
			s_BillingMessageEnum2Type.Add((int)BillingMessageEnum.LB_VerifyCode, typeof(LB_VerifyCode));
			s_Type2BillingMessageEnum.Add(typeof(LB_VerifyCode), (int)BillingMessageEnum.LB_VerifyCode);
		}

		private static Dictionary<int, Type> s_BillingMessageEnum2Type = new Dictionary<int, Type>();
		private static Dictionary<Type, int> s_Type2BillingMessageEnum = new Dictionary<Type, int>();
	}
}

