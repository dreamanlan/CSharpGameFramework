
using System;
using System.Collections.Generic;

namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal int checkDelegate(IntPtr l,int p,out Dsl.Common.GetTokenDelegation ua) {
            int op = extractFunction(l,p);
			if(LuaDLL.lua_isnil(l,p)) {
				ua=null;
				return op;
			}
            else if (LuaDLL.lua_isuserdata(l, p)==1)
            {
                ua = (Dsl.Common.GetTokenDelegation)checkObj(l, p);
                return op;
            }
            if(LuaDLL.lua_isnil(l,-1)) {
				ua=null;
				return op;
			}
            LuaDelegate ld;
            checkType(l, -1, out ld);
			LuaDLL.lua_pop(l,1);
            if(ld.d!=null)
            {
                ua = (Dsl.Common.GetTokenDelegation)ld.d;
                return op;
            }
			
			l = LuaState.get(l).L;
            ua = (ref Dsl.Common.DslToken a1,ref System.String a2,ref System.Int16 a3,ref System.Int32 a4) =>
            {
                int error = pushTry(l);

				pushValue(l, a1);
				pushValue(l, a2);
				pushValue(l, a3);
				pushValue(l, a4);
				ld.pcall(4, error);
				bool ret;
				checkType(l,error+1,out ret);
				checkValueType(l,error+2,out a1);
				checkType(l,error+3,out a2);
				checkType(l,error+4,out a3);
				checkType(l,error+5,out a4);
				LuaDLL.lua_settop(l, error-1);
				return ret;
			};
			ld.d=ua;
			return op;
		}
	}
}
