
using System;
using System.Collections.Generic;

namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal int checkDelegate(IntPtr l,int p,out Dsl.Common.BeforeEndStatementDelegation ua) {
            int op = extractFunction(l,p);
			if(LuaDLL.lua_isnil(l,p)) {
				ua=null;
				return op;
			}
            else if (LuaDLL.lua_isuserdata(l, p)==1)
            {
                ua = (Dsl.Common.BeforeEndStatementDelegation)checkObj(l, p);
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
                ua = (Dsl.Common.BeforeEndStatementDelegation)ld.d;
                return op;
            }
			
			l = LuaState.get(l).L;
            ua = (ref Dsl.Common.DslAction a1) =>
            {
                int error = pushTry(l);

				pushValue(l, a1);
				ld.pcall(1, error);
				bool ret;
				checkType(l,error+1,out ret);
				checkValueType(l,error+2,out a1);
				LuaDLL.lua_settop(l, error-1);
				return ret;
			};
			ld.d=ua;
			return op;
		}
	}
}
