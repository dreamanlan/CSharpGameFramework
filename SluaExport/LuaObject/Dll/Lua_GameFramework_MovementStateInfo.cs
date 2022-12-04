using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_GameFramework_MovementStateInfo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			GameFramework.MovementStateInfo o;
			o=new GameFramework.MovementStateInfo();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CalcDistancSquareToTarget(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			var ret=self.CalcDistancSquareToTarget();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetPosition__Vector3(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			ScriptRuntime.Vector3 a1;
			checkValueType(l,2,out a1);
			self.SetPosition(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetPosition__Single__Single__Single(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			self.SetPosition(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetPosition3D(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			var ret=self.GetPosition3D();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetPosition2D__Vector2(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			ScriptRuntime.Vector2 a1;
			checkValueType(l,2,out a1);
			self.SetPosition2D(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetPosition2D__Single__Single(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			self.SetPosition2D(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetPosition2D(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			var ret=self.GetPosition2D();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetFaceDir__Single(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			self.SetFaceDir(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetFaceDir__Single__Boolean(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			System.Boolean a2;
			checkType(l,3,out a2);
			self.SetFaceDir(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetFaceDir(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			var ret=self.GetFaceDir();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SetWantedFaceDir(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			self.SetWantedFaceDir(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetWantedFaceDir(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			var ret=self.GetWantedFaceDir();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetFaceDir2D(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			var ret=self.GetFaceDir2D();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetFaceDir3D(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			var ret=self.GetFaceDir3D();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Reset(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsMoving(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsMoving);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsMoving(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsMoving=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_FormationIndex(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.FormationIndex);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_FormationIndex(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.FormationIndex=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TargetPosition(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TargetPosition);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_TargetPosition(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.TargetPosition=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_PositionX(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PositionX);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_PositionX(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.PositionX=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_PositionY(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PositionY);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_PositionY(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.PositionY=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_PositionZ(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.PositionZ);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_PositionZ(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.PositionZ=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_TargetDir(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.TargetDir);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_LastTargetPos(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.LastTargetPos);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_LastTargetPos(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			ScriptRuntime.Vector3 v;
			checkValueType(l,2,out v);
			self.LastTargetPos=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsFaceDirChanged(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsFaceDirChanged);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsFaceDirChanged(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsFaceDirChanged=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsMoveStatusChanged(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.IsMoveStatusChanged);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_IsMoveStatusChanged(IntPtr l) {
		try {
			GameFramework.MovementStateInfo self=(GameFramework.MovementStateInfo)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.IsMoveStatusChanged=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"GameFramework.MovementStateInfo");
		addMember(l,ctor_s);
		addMember(l,CalcDistancSquareToTarget);
		addMember(l,SetPosition__Vector3);
		addMember(l,SetPosition__Single__Single__Single);
		addMember(l,GetPosition3D);
		addMember(l,SetPosition2D__Vector2);
		addMember(l,SetPosition2D__Single__Single);
		addMember(l,GetPosition2D);
		addMember(l,SetFaceDir__Single);
		addMember(l,SetFaceDir__Single__Boolean);
		addMember(l,GetFaceDir);
		addMember(l,SetWantedFaceDir);
		addMember(l,GetWantedFaceDir);
		addMember(l,GetFaceDir2D);
		addMember(l,GetFaceDir3D);
		addMember(l,Reset);
		addMember(l,"IsMoving",get_IsMoving,set_IsMoving,true);
		addMember(l,"FormationIndex",get_FormationIndex,set_FormationIndex,true);
		addMember(l,"TargetPosition",get_TargetPosition,set_TargetPosition,true);
		addMember(l,"PositionX",get_PositionX,set_PositionX,true);
		addMember(l,"PositionY",get_PositionY,set_PositionY,true);
		addMember(l,"PositionZ",get_PositionZ,set_PositionZ,true);
		addMember(l,"TargetDir",get_TargetDir,null,true);
		addMember(l,"LastTargetPos",get_LastTargetPos,set_LastTargetPos,true);
		addMember(l,"IsFaceDirChanged",get_IsFaceDirChanged,set_IsFaceDirChanged,true);
		addMember(l,"IsMoveStatusChanged",get_IsMoveStatusChanged,set_IsMoveStatusChanged,true);
		createTypeMetatable(l,null, typeof(GameFramework.MovementStateInfo));
	}
}
