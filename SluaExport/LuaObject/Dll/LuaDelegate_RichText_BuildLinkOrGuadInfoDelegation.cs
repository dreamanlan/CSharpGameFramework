
using System;
using System.Collections.Generic;

namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal int checkDelegate(IntPtr l,int p,out RichText.BuildLinkOrGuadInfoDelegation ua) {
            int op = extractFunction(l,p);
			if(LuaDLL.lua_isnil(l,p)) {
				ua=null;
				return op;
			}
            else if (LuaDLL.lua_isuserdata(l, p)==1)
            {
                ua = (RichText.BuildLinkOrGuadInfoDelegation)checkObj(l, p);
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
                ua = (RichText.BuildLinkOrGuadInfoDelegation)ld.d;
                return op;
            }
			
			l = LuaState.get(l).L;
            ua = (string a1,string a2,RichTextParser.HyperText a3,out RichText.LinkInfo a4,out RichText.QuadInfo a5) =>
            {
                int error = pushTry(l);

				pushValue(l, a1);
				pushValue(l, a2);
				pushValue(l, a3);
				LuaDLL.lua_pushnil(l);
				LuaDLL.lua_pushnil(l);
				ld.pcall(5, error);
				bool ret;
				checkType(l,error+1,out ret);
				checkType(l,error+2,out a4);
				checkType(l,error+3,out a5);
				LuaDLL.lua_settop(l, error-1);
				return ret;
			};
			ld.d=ua;
			return op;
		}
	}
}
