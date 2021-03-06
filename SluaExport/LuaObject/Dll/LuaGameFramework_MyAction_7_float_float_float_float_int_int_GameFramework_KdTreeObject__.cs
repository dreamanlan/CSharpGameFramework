﻿
using System;
using System.Collections.Generic;


namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal int checkDelegate(IntPtr l,int p,out GameFramework.MyAction<System.Single,System.Single,System.Single,System.Single,System.Int32,System.Int32,GameFramework.KdTreeObject[]> ua) {
            int op = extractFunction(l,p);
			if(LuaDLL.lua_isnil(l,p)) {
				ua=null;
				return op;
			}
            else if (LuaDLL.lua_isuserdata(l, p)==1)
            {
                ua = (GameFramework.MyAction<System.Single,System.Single,System.Single,System.Single,System.Int32,System.Int32,GameFramework.KdTreeObject[]>)checkObj(l, p);
                return op;
            }
            LuaDelegate ld;
            checkType(l, -1, out ld);
            if(ld.d!=null)
            {
                ua = (GameFramework.MyAction<System.Single,System.Single,System.Single,System.Single,System.Int32,System.Int32,GameFramework.KdTreeObject[]>)ld.d;
                return op;
            }
			LuaDLL.lua_pop(l,1);
			
			l = LuaState.get(l).L;
            ua = (float a1,float a2,float a3,float a4,int a5,int a6,GameFramework.KdTreeObject[] a7) =>
            {
                int error = pushTry(l);

				pushValue(l,a1);
				pushValue(l,a2);
				pushValue(l,a3);
				pushValue(l,a4);
				pushValue(l,a5);
				pushValue(l,a6);
				pushValue(l,a7);
				ld.pcall(7, error);
				LuaDLL.lua_settop(l, error-1);
			};
			ld.d=ua;
			return op;
		}
	}
}
