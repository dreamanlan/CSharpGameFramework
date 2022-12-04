
using System;
using System.Collections.Generic;

namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal int checkDelegate(IntPtr l,int p,out Dsl.Common.AddFunctionDelegation ua) {
            int op = extractFunction(l,p);
			if(LuaDLL.lua_isnil(l,p)) {
				ua=null;
				return op;
			}
            else if (LuaDLL.lua_isuserdata(l, p)==1)
            {
                ua = (Dsl.Common.AddFunctionDelegation)checkObj(l, p);
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
                ua = (Dsl.Common.AddFunctionDelegation)ld.d;
                return op;
            }
			
			l = LuaState.get(l).L;
            ua = (ref Dsl.Common.DslAction a1,Dsl.StatementData a2,Dsl.FunctionData a3) =>
            {
                int error = pushTry(l);

				pushValue(l, a1);
				pushValue(l, a2);
				pushValue(l, a3);
				ld.pcall(3, error);
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
