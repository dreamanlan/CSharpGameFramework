﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_PhysicsScene : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			UnityEngine.PhysicsScene o;
			o=new UnityEngine.PhysicsScene();
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
	static new public int ToString(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			var ret=self.ToString();
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
	static public int Equals__Object(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.Equals(a1);
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
	static public int Equals__PhysicsScene(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.PhysicsScene a1;
			checkValueType(l,2,out a1);
			var ret=self.Equals(a1);
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
	static public int IsValid(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			var ret=self.IsValid();
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
	static public int IsEmpty(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			var ret=self.IsEmpty();
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
	static public int Simulate(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			System.Single a1;
			checkType(l,2,out a1);
			self.Simulate(a1);
			pushValue(l,true);
			setBack(l,(object)self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Raycast__Vector3__Vector3__Single__Int32__QueryTriggerInteraction(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			System.Int32 a4;
			checkType(l,5,out a4);
			UnityEngine.QueryTriggerInteraction a5;
			checkEnum(l,6,out a5);
			var ret=self.Raycast(a1,a2,a3,a4,a5);
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
	static public int Raycast__Vector3__Vector3__O_RaycastHit__Single__Int32__QueryTriggerInteraction(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,3,out a2);
			UnityEngine.RaycastHit a3;
			System.Single a4;
			checkType(l,5,out a4);
			System.Int32 a5;
			checkType(l,6,out a5);
			UnityEngine.QueryTriggerInteraction a6;
			checkEnum(l,7,out a6);
			var ret=self.Raycast(a1,a2,out a3,a4,a5,a6);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a3);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Raycast__Vector3__Vector3__A_RaycastHit__Single__Int32__QueryTriggerInteraction(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,3,out a2);
			UnityEngine.RaycastHit[] a3;
			checkArray(l,4,out a3);
			System.Single a4;
			checkType(l,5,out a4);
			System.Int32 a5;
			checkType(l,6,out a5);
			UnityEngine.QueryTriggerInteraction a6;
			checkEnum(l,7,out a6);
			var ret=self.Raycast(a1,a2,a3,a4,a5,a6);
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
	static public int CapsuleCast__Vector3__Vector3__Single__Vector3__O_RaycastHit__Single__Int32__QueryTriggerInteraction(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			UnityEngine.Vector3 a4;
			checkType(l,5,out a4);
			UnityEngine.RaycastHit a5;
			System.Single a6;
			checkType(l,7,out a6);
			System.Int32 a7;
			checkType(l,8,out a7);
			UnityEngine.QueryTriggerInteraction a8;
			checkEnum(l,9,out a8);
			var ret=self.CapsuleCast(a1,a2,a3,a4,out a5,a6,a7,a8);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a5);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CapsuleCast__Vector3__Vector3__Single__Vector3__A_RaycastHit__Single__Int32__QueryTriggerInteraction(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			UnityEngine.Vector3 a4;
			checkType(l,5,out a4);
			UnityEngine.RaycastHit[] a5;
			checkArray(l,6,out a5);
			System.Single a6;
			checkType(l,7,out a6);
			System.Int32 a7;
			checkType(l,8,out a7);
			UnityEngine.QueryTriggerInteraction a8;
			checkEnum(l,9,out a8);
			var ret=self.CapsuleCast(a1,a2,a3,a4,a5,a6,a7,a8);
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
	static public int OverlapCapsule(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			UnityEngine.Collider[] a4;
			checkArray(l,5,out a4);
			System.Int32 a5;
			checkType(l,6,out a5);
			UnityEngine.QueryTriggerInteraction a6;
			checkEnum(l,7,out a6);
			var ret=self.OverlapCapsule(a1,a2,a3,a4,a5,a6);
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
	static public int SphereCast__Vector3__Single__Vector3__O_RaycastHit__Single__Int32__QueryTriggerInteraction(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			UnityEngine.Vector3 a3;
			checkType(l,4,out a3);
			UnityEngine.RaycastHit a4;
			System.Single a5;
			checkType(l,6,out a5);
			System.Int32 a6;
			checkType(l,7,out a6);
			UnityEngine.QueryTriggerInteraction a7;
			checkEnum(l,8,out a7);
			var ret=self.SphereCast(a1,a2,a3,out a4,a5,a6,a7);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a4);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int SphereCast__Vector3__Single__Vector3__A_RaycastHit__Single__Int32__QueryTriggerInteraction(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			UnityEngine.Vector3 a3;
			checkType(l,4,out a3);
			UnityEngine.RaycastHit[] a4;
			checkArray(l,5,out a4);
			System.Single a5;
			checkType(l,6,out a5);
			System.Int32 a6;
			checkType(l,7,out a6);
			UnityEngine.QueryTriggerInteraction a7;
			checkEnum(l,8,out a7);
			var ret=self.SphereCast(a1,a2,a3,a4,a5,a6,a7);
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
	static public int OverlapSphere(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			UnityEngine.Collider[] a3;
			checkArray(l,4,out a3);
			System.Int32 a4;
			checkType(l,5,out a4);
			UnityEngine.QueryTriggerInteraction a5;
			checkEnum(l,6,out a5);
			var ret=self.OverlapSphere(a1,a2,a3,a4,a5);
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
	static public int BoxCast__Vector3__Vector3__Vector3__O_RaycastHit(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,3,out a2);
			UnityEngine.Vector3 a3;
			checkType(l,4,out a3);
			UnityEngine.RaycastHit a4;
			var ret=self.BoxCast(a1,a2,a3,out a4);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a4);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int BoxCast__Vector3__Vector3__Vector3__A_RaycastHit(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,3,out a2);
			UnityEngine.Vector3 a3;
			checkType(l,4,out a3);
			UnityEngine.RaycastHit[] a4;
			checkArray(l,5,out a4);
			var ret=self.BoxCast(a1,a2,a3,a4);
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
	static public int BoxCast__Vector3__Vector3__Vector3__O_RaycastHit__Quaternion__Single__Int32__QueryTriggerInteraction(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,3,out a2);
			UnityEngine.Vector3 a3;
			checkType(l,4,out a3);
			UnityEngine.RaycastHit a4;
			UnityEngine.Quaternion a5;
			checkType(l,6,out a5);
			System.Single a6;
			checkType(l,7,out a6);
			System.Int32 a7;
			checkType(l,8,out a7);
			UnityEngine.QueryTriggerInteraction a8;
			checkEnum(l,9,out a8);
			var ret=self.BoxCast(a1,a2,a3,out a4,a5,a6,a7,a8);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a4);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int BoxCast__Vector3__Vector3__Vector3__A_RaycastHit__Quaternion__Single__Int32__QueryTriggerInteraction(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,3,out a2);
			UnityEngine.Vector3 a3;
			checkType(l,4,out a3);
			UnityEngine.RaycastHit[] a4;
			checkArray(l,5,out a4);
			UnityEngine.Quaternion a5;
			checkType(l,6,out a5);
			System.Single a6;
			checkType(l,7,out a6);
			System.Int32 a7;
			checkType(l,8,out a7);
			UnityEngine.QueryTriggerInteraction a8;
			checkEnum(l,9,out a8);
			var ret=self.BoxCast(a1,a2,a3,a4,a5,a6,a7,a8);
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
	static public int OverlapBox__Vector3__Vector3__A_Collider(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,3,out a2);
			UnityEngine.Collider[] a3;
			checkArray(l,4,out a3);
			var ret=self.OverlapBox(a1,a2,a3);
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
	static public int OverlapBox__Vector3__Vector3__A_Collider__Quaternion__Int32__QueryTriggerInteraction(IntPtr l) {
		try {
			UnityEngine.PhysicsScene self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 a1;
			checkType(l,2,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,3,out a2);
			UnityEngine.Collider[] a3;
			checkArray(l,4,out a3);
			UnityEngine.Quaternion a4;
			checkType(l,5,out a4);
			System.Int32 a5;
			checkType(l,6,out a5);
			UnityEngine.QueryTriggerInteraction a6;
			checkEnum(l,7,out a6);
			var ret=self.OverlapBox(a1,a2,a3,a4,a5,a6);
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
	static public int op_Equality_s(IntPtr l) {
		try {
			UnityEngine.PhysicsScene a1;
			checkValueType(l,1,out a1);
			UnityEngine.PhysicsScene a2;
			checkValueType(l,2,out a2);
			var ret=(a1==a2);
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
	static public int op_Inequality_s(IntPtr l) {
		try {
			UnityEngine.PhysicsScene a1;
			checkValueType(l,1,out a1);
			UnityEngine.PhysicsScene a2;
			checkValueType(l,2,out a2);
			var ret=(a1!=a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.PhysicsScene");
		addMember(l,ctor_s);
		addMember(l,ToString);
		addMember(l,Equals__Object);
		addMember(l,Equals__PhysicsScene);
		addMember(l,IsValid);
		addMember(l,IsEmpty);
		addMember(l,Simulate);
		addMember(l,Raycast__Vector3__Vector3__Single__Int32__QueryTriggerInteraction);
		addMember(l,Raycast__Vector3__Vector3__O_RaycastHit__Single__Int32__QueryTriggerInteraction);
		addMember(l,Raycast__Vector3__Vector3__A_RaycastHit__Single__Int32__QueryTriggerInteraction);
		addMember(l,CapsuleCast__Vector3__Vector3__Single__Vector3__O_RaycastHit__Single__Int32__QueryTriggerInteraction);
		addMember(l,CapsuleCast__Vector3__Vector3__Single__Vector3__A_RaycastHit__Single__Int32__QueryTriggerInteraction);
		addMember(l,OverlapCapsule);
		addMember(l,SphereCast__Vector3__Single__Vector3__O_RaycastHit__Single__Int32__QueryTriggerInteraction);
		addMember(l,SphereCast__Vector3__Single__Vector3__A_RaycastHit__Single__Int32__QueryTriggerInteraction);
		addMember(l,OverlapSphere);
		addMember(l,BoxCast__Vector3__Vector3__Vector3__O_RaycastHit);
		addMember(l,BoxCast__Vector3__Vector3__Vector3__A_RaycastHit);
		addMember(l,BoxCast__Vector3__Vector3__Vector3__O_RaycastHit__Quaternion__Single__Int32__QueryTriggerInteraction);
		addMember(l,BoxCast__Vector3__Vector3__Vector3__A_RaycastHit__Quaternion__Single__Int32__QueryTriggerInteraction);
		addMember(l,OverlapBox__Vector3__Vector3__A_Collider);
		addMember(l,OverlapBox__Vector3__Vector3__A_Collider__Quaternion__Int32__QueryTriggerInteraction);
		addMember(l,op_Equality_s);
		addMember(l,op_Inequality_s);
		createTypeMetatable(l,null, typeof(UnityEngine.PhysicsScene),typeof(System.ValueType));
	}
}