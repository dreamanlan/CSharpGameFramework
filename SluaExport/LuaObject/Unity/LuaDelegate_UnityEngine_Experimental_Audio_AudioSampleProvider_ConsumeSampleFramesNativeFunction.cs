﻿
using System;
using System.Collections.Generic;

namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal int checkDelegate(IntPtr l,int p,out UnityEngine.Experimental.Audio.AudioSampleProvider.ConsumeSampleFramesNativeFunction ua) {
            int op = extractFunction(l,p);
			if(LuaDLL.lua_isnil(l,p)) {
				ua=null;
				return op;
			}
            else if (LuaDLL.lua_isuserdata(l, p)==1)
            {
                ua = (UnityEngine.Experimental.Audio.AudioSampleProvider.ConsumeSampleFramesNativeFunction)checkObj(l, p);
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
                ua = (UnityEngine.Experimental.Audio.AudioSampleProvider.ConsumeSampleFramesNativeFunction)ld.d;
                return op;
            }
			
			l = LuaState.get(l).L;
            ua = (System.UInt32 a1,System.IntPtr a2,System.UInt32 a3) =>
            {
                int error = pushTry(l);

				pushValue(l, a1);
				pushValue(l, a2);
				pushValue(l, a3);
				ld.pcall(3, error);
				System.UInt32 ret;
				checkType(l,error+1,out ret);
				LuaDLL.lua_settop(l, error-1);
				return ret;
			};
			ld.d=ua;
			return op;
		}
	}
}