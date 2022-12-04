
using System;
using System.Collections.Generic;

namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal int checkDelegate(IntPtr l,int p,out StorySystem.StoryCommandDebuggerDelegation ua) {
            int op = extractFunction(l,p);
			if(LuaDLL.lua_isnil(l,p)) {
				ua=null;
				return op;
			}
            else if (LuaDLL.lua_isuserdata(l, p)==1)
            {
                ua = (StorySystem.StoryCommandDebuggerDelegation)checkObj(l, p);
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
                ua = (StorySystem.StoryCommandDebuggerDelegation)ld.d;
                return op;
            }
			
			l = LuaState.get(l).L;
            ua = (StorySystem.StoryInstance a1,StorySystem.StoryMessageHandler a2,StorySystem.IStoryCommand a3,System.Int64 a4,BoxedValue a5,BoxedValueList a6) =>
            {
                int error = pushTry(l);

				pushValue(l, a1);
				pushValue(l, a2);
				pushValue(l, a3);
				pushValue(l, a4);
				pushValue(l, a5.GetObject());
				pushValue(l, a6);
				ld.pcall(6, error);
				bool ret;
				checkType(l,error+1,out ret);
				LuaDLL.lua_settop(l, error-1);
				return ret;
			};
			ld.d=ua;
			return op;
		}
	}
}
