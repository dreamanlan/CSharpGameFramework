using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_BoxedValue : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int ctor_s(IntPtr l) {
		try {
			BoxedValue o;
			o=new BoxedValue();
			pushValue(l,true);
			pushValue(l,o.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetTypeName(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetTypeName();
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
	static public int As(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Type a1;
			checkType(l,2,out a1);
			var ret=self.As(a1);
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
	static public int SetNullObject(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			self.SetNullObject();
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
	static public int SetNullString(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			self.SetNullString();
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
	static public int SetEmptyString(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			self.SetEmptyString();
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
	static public int Set__Boolean(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__ColorF(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			ScriptRuntime.ColorF a1;
			checkValueType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__Quaternion(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			ScriptRuntime.Quaternion a1;
			checkValueType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__Vector4(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector4 a1;
			checkValueType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__Vector3(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector3 a1;
			checkValueType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__Vector2(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			ScriptRuntime.Vector2 a1;
			checkValueType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__Decimal(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Decimal a1;
			checkValueType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__Color32(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			ScriptRuntime.Color32 a1;
			checkValueType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__UInt64(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.UInt64 a1;
			checkType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__UInt16(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.UInt16 a1;
			checkType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__Byte(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Byte a1;
			checkType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__Int64(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Int64 a1;
			checkType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__Int32(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__Int16(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Int16 a1;
			checkType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__SByte(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.SByte a1;
			checkType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__Char(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Char a1;
			checkType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__UInt32(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.UInt32 a1;
			checkType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__String(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.String a1;
			checkType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__Single(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Single a1;
			checkType(l,2,out a1);
			self.Set(a1);
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
	static public int Set__Double(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Double a1;
			checkType(l,2,out a1);
			self.Set(a1);
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
	static public int SetObject(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Object a1;
			checkType(l,2,out a1);
			self.SetObject(a1);
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
	static public int SetWithObjectType(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Object a1;
			checkType(l,2,out a1);
			self.SetWithObjectType(a1);
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
	static public int GetBool(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetBool();
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
	static public int GetChar(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetChar();
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
	static public int GetSByte(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetSByte();
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
	static public int GetShort(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetShort();
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
	static public int GetInt(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetInt();
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
	static public int GetLong(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetLong();
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
	static public int GetByte(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetByte();
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
	static public int GetUShort(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetUShort();
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
	static public int GetUInt(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetUInt();
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
	static public int GetULong(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetULong();
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
	static public int GetFloat(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetFloat();
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
	static public int GetDouble(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetDouble();
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
	static public int GetDecimal(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetDecimal();
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
	static public int GetVector2(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetVector2();
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
	static public int GetVector3(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetVector3();
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
	static public int GetVector4(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetVector4();
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
	static public int GetQuaternion(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetQuaternion();
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
	static public int GetColor(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetColor();
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
	static public int GetColor32(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetColor32();
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
	static public int GetString(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetString();
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
	static public int GetObject(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetObject();
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
	static public int CastTo(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Type a1;
			checkType(l,2,out a1);
			var ret=self.CastTo(a1);
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
	static public int GenericSet(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Type a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.GenericSet(a1,a2);
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
	static public int SetBool(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Boolean a1;
			checkType(l,2,out a1);
			self.SetBool(a1);
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
	static public int SetInteger(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Int64 a1;
			checkType(l,2,out a1);
			self.SetInteger(a1);
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
	static public int SetNumber(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Double a1;
			checkType(l,2,out a1);
			self.SetNumber(a1);
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
	static public int SetString(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.String a1;
			checkType(l,2,out a1);
			self.SetString(a1);
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
	static public int GetInteger(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetInteger();
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
	static public int GetNumber(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			var ret=self.GetNumber();
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
	static public int CopyFrom(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			BoxedValue a1;
			checkValueType(l,2,out a1);
			self.CopyFrom(a1);
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
	static new public int ToString(IntPtr l) {
		try {
			BoxedValue self;
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
	static public int op_Implicit__BoxedValue__String_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__Decimal__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.Decimal ret=a1;
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
	static public int op_Implicit__BoxedValue__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__Vector2__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector2 ret=a1;
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
	static public int op_Implicit__BoxedValue__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__Vector3__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector3 ret=a1;
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
	static public int op_Implicit__BoxedValue__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__Vector4__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Vector4 ret=a1;
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
	static public int op_Implicit__BoxedValue__Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__Quaternion__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Quaternion ret=a1;
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
	static public int op_Implicit__BoxedValue__ColorF_s(IntPtr l) {
		try {
			ScriptRuntime.ColorF a1;
			checkValueType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__ColorF__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.ColorF ret=a1;
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
	static public int op_Implicit__BoxedValue__Color32_s(IntPtr l) {
		try {
			ScriptRuntime.Color32 a1;
			checkValueType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__Color32__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			ScriptRuntime.Color32 ret=a1;
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
	static public int op_Implicit__BoxedValue__A_Byte_s(IntPtr l) {
		try {
			System.Byte[] a1;
			checkArray(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__A_Byte__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.Byte[] ret=a1;
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
	static public int op_Implicit__BoxedValue__A_Object_s(IntPtr l) {
		try {
			System.Object[] a1;
			checkArray(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__A_Object__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.Object[] ret=a1;
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
	static public int op_Implicit__BoxedValue__Type_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__Type__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.Type ret=a1;
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
	static public int op_Implicit__BoxedValue__ObjList_s(IntPtr l) {
		try {
			ObjList a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__ObjList__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			ObjList ret=a1;
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
	static public int op_Implicit__BoxedValue__Decimal_s(IntPtr l) {
		try {
			System.Decimal a1;
			checkValueType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__Double__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.Double ret=a1;
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
	static public int op_Implicit__ArrayList__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.Collections.ArrayList ret=a1;
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
	static public int op_Implicit__Single__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.Single ret=a1;
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
	static public int op_Implicit__String__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.String ret=a1;
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
	static public int op_Implicit__BoxedValue__Boolean_s(IntPtr l) {
		try {
			System.Boolean a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__Boolean__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.Boolean ret=a1;
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
	static public int op_Implicit__BoxedValue__Char_s(IntPtr l) {
		try {
			System.Char a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__Char__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.Char ret=a1;
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
	static public int op_Implicit__BoxedValue__SByte_s(IntPtr l) {
		try {
			System.SByte a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__SByte__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.SByte ret=a1;
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
	static public int op_Implicit__BoxedValue__Int16_s(IntPtr l) {
		try {
			System.Int16 a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__Int16__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.Int16 ret=a1;
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
	static public int op_Implicit__BoxedValue__Int32_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__BoxedValue__ArrayList_s(IntPtr l) {
		try {
			System.Collections.ArrayList a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__Int32__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.Int32 ret=a1;
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
	static public int op_Implicit__Int64__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.Int64 ret=a1;
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
	static public int op_Implicit__BoxedValue__Byte_s(IntPtr l) {
		try {
			System.Byte a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__Byte__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.Byte ret=a1;
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
	static public int op_Implicit__BoxedValue__UInt16_s(IntPtr l) {
		try {
			System.UInt16 a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__UInt16__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.UInt16 ret=a1;
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
	static public int op_Implicit__BoxedValue__UInt32_s(IntPtr l) {
		try {
			System.UInt32 a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__UInt32__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.UInt32 ret=a1;
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
	static public int op_Implicit__BoxedValue__UInt64_s(IntPtr l) {
		try {
			System.UInt64 a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__UInt64__BoxedValue_s(IntPtr l) {
		try {
			BoxedValue a1;
			checkValueType(l,1,out a1);
			System.UInt64 ret=a1;
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
	static public int op_Implicit__BoxedValue__Int64_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__BoxedValue__Single_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int op_Implicit__BoxedValue__Double_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			BoxedValue ret=a1;
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__Boolean_s(IntPtr l) {
		try {
			System.Boolean a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__ColorF_s(IntPtr l) {
		try {
			ScriptRuntime.ColorF a1;
			checkValueType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__Quaternion_s(IntPtr l) {
		try {
			ScriptRuntime.Quaternion a1;
			checkValueType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__Vector4_s(IntPtr l) {
		try {
			ScriptRuntime.Vector4 a1;
			checkValueType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__Vector3_s(IntPtr l) {
		try {
			ScriptRuntime.Vector3 a1;
			checkValueType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__Vector2_s(IntPtr l) {
		try {
			ScriptRuntime.Vector2 a1;
			checkValueType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__Decimal_s(IntPtr l) {
		try {
			System.Decimal a1;
			checkValueType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__Color32_s(IntPtr l) {
		try {
			ScriptRuntime.Color32 a1;
			checkValueType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__String_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__UInt32_s(IntPtr l) {
		try {
			System.UInt32 a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__UInt16_s(IntPtr l) {
		try {
			System.UInt16 a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__Byte_s(IntPtr l) {
		try {
			System.Byte a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__Int64_s(IntPtr l) {
		try {
			System.Int64 a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__Int32_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__Int16_s(IntPtr l) {
		try {
			System.Int16 a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__SByte_s(IntPtr l) {
		try {
			System.SByte a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__UInt64_s(IntPtr l) {
		try {
			System.UInt64 a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__Double_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int From__Single_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.From(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FromBool_s(IntPtr l) {
		try {
			System.Boolean a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.FromBool(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FromNumber_s(IntPtr l) {
		try {
			System.Double a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.FromNumber(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FromString_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.FromString(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int FromObject_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			var ret=BoxedValue.FromObject(a1);
			pushValue(l,true);
			pushValue(l,ret.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_ObjectType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_ObjectType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_StringType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_StringType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_BoolType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_BoolType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_CharType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_CharType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_SByteType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_SByteType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_ShortType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_ShortType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_IntType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_IntType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_LongType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_LongType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_ByteType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_ByteType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_UShortType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_UShortType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_UIntType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_UIntType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_ULongType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_ULongType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_FloatType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_FloatType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_DoubleType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_DoubleType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_DecimalType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_DecimalType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_Vector2Type(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_Vector2Type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_Vector3Type(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_Vector3Type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_Vector4Type(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_Vector4Type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_QuaternionType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_QuaternionType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_ColorType(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_ColorType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_c_Color32Type(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.c_Color32Type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_Type(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.Type);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_Type(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Int32 v;
			checkType(l,2,out v);
			self.Type=v;
			setBack(l,(object)self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_ObjectVal(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.ObjectVal);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_ObjectVal(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			System.Object v;
			checkType(l,2,out v);
			self.ObjectVal=v;
			setBack(l,(object)self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_StringVal(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.StringVal);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_StringVal(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			string v;
			checkType(l,2,out v);
			self.StringVal=v;
			setBack(l,(object)self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsNullObject(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.IsNullObject);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsNullOrEmptyString(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.IsNullOrEmptyString);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsObject(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.IsObject);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsString(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.IsString);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsBoolean(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.IsBoolean);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsChar(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.IsChar);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsInteger(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.IsInteger);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_IsNumber(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.IsNumber);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_AsString(IntPtr l) {
		try {
			BoxedValue self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.AsString);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_NullObject(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.NullObject.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_EmptyString(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,BoxedValue.EmptyString.GetObject());
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"BoxedValue");
		addMember(l,ctor_s);
		addMember(l,GetTypeName);
		addMember(l,As);
		addMember(l,SetNullObject);
		addMember(l,SetNullString);
		addMember(l,SetEmptyString);
		addMember(l,Set__Boolean);
		addMember(l,Set__ColorF);
		addMember(l,Set__Quaternion);
		addMember(l,Set__Vector4);
		addMember(l,Set__Vector3);
		addMember(l,Set__Vector2);
		addMember(l,Set__Decimal);
		addMember(l,Set__Color32);
		addMember(l,Set__UInt64);
		addMember(l,Set__UInt16);
		addMember(l,Set__Byte);
		addMember(l,Set__Int64);
		addMember(l,Set__Int32);
		addMember(l,Set__Int16);
		addMember(l,Set__SByte);
		addMember(l,Set__Char);
		addMember(l,Set__UInt32);
		addMember(l,Set__String);
		addMember(l,Set__Single);
		addMember(l,Set__Double);
		addMember(l,SetObject);
		addMember(l,SetWithObjectType);
		addMember(l,GetBool);
		addMember(l,GetChar);
		addMember(l,GetSByte);
		addMember(l,GetShort);
		addMember(l,GetInt);
		addMember(l,GetLong);
		addMember(l,GetByte);
		addMember(l,GetUShort);
		addMember(l,GetUInt);
		addMember(l,GetULong);
		addMember(l,GetFloat);
		addMember(l,GetDouble);
		addMember(l,GetDecimal);
		addMember(l,GetVector2);
		addMember(l,GetVector3);
		addMember(l,GetVector4);
		addMember(l,GetQuaternion);
		addMember(l,GetColor);
		addMember(l,GetColor32);
		addMember(l,GetString);
		addMember(l,GetObject);
		addMember(l,CastTo);
		addMember(l,GenericSet);
		addMember(l,SetBool);
		addMember(l,SetInteger);
		addMember(l,SetNumber);
		addMember(l,SetString);
		addMember(l,GetInteger);
		addMember(l,GetNumber);
		addMember(l,CopyFrom);
		addMember(l,ToString);
		addMember(l,op_Implicit__BoxedValue__String_s);
		addMember(l,op_Implicit__Decimal__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__Vector2_s);
		addMember(l,op_Implicit__Vector2__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__Vector3_s);
		addMember(l,op_Implicit__Vector3__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__Vector4_s);
		addMember(l,op_Implicit__Vector4__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__Quaternion_s);
		addMember(l,op_Implicit__Quaternion__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__ColorF_s);
		addMember(l,op_Implicit__ColorF__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__Color32_s);
		addMember(l,op_Implicit__Color32__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__A_Byte_s);
		addMember(l,op_Implicit__A_Byte__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__A_Object_s);
		addMember(l,op_Implicit__A_Object__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__Type_s);
		addMember(l,op_Implicit__Type__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__ObjList_s);
		addMember(l,op_Implicit__ObjList__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__Decimal_s);
		addMember(l,op_Implicit__Double__BoxedValue_s);
		addMember(l,op_Implicit__ArrayList__BoxedValue_s);
		addMember(l,op_Implicit__Single__BoxedValue_s);
		addMember(l,op_Implicit__String__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__Boolean_s);
		addMember(l,op_Implicit__Boolean__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__Char_s);
		addMember(l,op_Implicit__Char__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__SByte_s);
		addMember(l,op_Implicit__SByte__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__Int16_s);
		addMember(l,op_Implicit__Int16__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__Int32_s);
		addMember(l,op_Implicit__BoxedValue__ArrayList_s);
		addMember(l,op_Implicit__Int32__BoxedValue_s);
		addMember(l,op_Implicit__Int64__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__Byte_s);
		addMember(l,op_Implicit__Byte__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__UInt16_s);
		addMember(l,op_Implicit__UInt16__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__UInt32_s);
		addMember(l,op_Implicit__UInt32__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__UInt64_s);
		addMember(l,op_Implicit__UInt64__BoxedValue_s);
		addMember(l,op_Implicit__BoxedValue__Int64_s);
		addMember(l,op_Implicit__BoxedValue__Single_s);
		addMember(l,op_Implicit__BoxedValue__Double_s);
		addMember(l,From__Boolean_s);
		addMember(l,From__ColorF_s);
		addMember(l,From__Quaternion_s);
		addMember(l,From__Vector4_s);
		addMember(l,From__Vector3_s);
		addMember(l,From__Vector2_s);
		addMember(l,From__Decimal_s);
		addMember(l,From__Color32_s);
		addMember(l,From__String_s);
		addMember(l,From__UInt32_s);
		addMember(l,From__UInt16_s);
		addMember(l,From__Byte_s);
		addMember(l,From__Int64_s);
		addMember(l,From__Int32_s);
		addMember(l,From__Int16_s);
		addMember(l,From__SByte_s);
		addMember(l,From__UInt64_s);
		addMember(l,From__Double_s);
		addMember(l,From__Single_s);
		addMember(l,FromBool_s);
		addMember(l,FromNumber_s);
		addMember(l,FromString_s);
		addMember(l,FromObject_s);
		addMember(l,"c_ObjectType",get_c_ObjectType,null,false);
		addMember(l,"c_StringType",get_c_StringType,null,false);
		addMember(l,"c_BoolType",get_c_BoolType,null,false);
		addMember(l,"c_CharType",get_c_CharType,null,false);
		addMember(l,"c_SByteType",get_c_SByteType,null,false);
		addMember(l,"c_ShortType",get_c_ShortType,null,false);
		addMember(l,"c_IntType",get_c_IntType,null,false);
		addMember(l,"c_LongType",get_c_LongType,null,false);
		addMember(l,"c_ByteType",get_c_ByteType,null,false);
		addMember(l,"c_UShortType",get_c_UShortType,null,false);
		addMember(l,"c_UIntType",get_c_UIntType,null,false);
		addMember(l,"c_ULongType",get_c_ULongType,null,false);
		addMember(l,"c_FloatType",get_c_FloatType,null,false);
		addMember(l,"c_DoubleType",get_c_DoubleType,null,false);
		addMember(l,"c_DecimalType",get_c_DecimalType,null,false);
		addMember(l,"c_Vector2Type",get_c_Vector2Type,null,false);
		addMember(l,"c_Vector3Type",get_c_Vector3Type,null,false);
		addMember(l,"c_Vector4Type",get_c_Vector4Type,null,false);
		addMember(l,"c_QuaternionType",get_c_QuaternionType,null,false);
		addMember(l,"c_ColorType",get_c_ColorType,null,false);
		addMember(l,"c_Color32Type",get_c_Color32Type,null,false);
		addMember(l,"Type",get_Type,set_Type,true);
		addMember(l,"ObjectVal",get_ObjectVal,set_ObjectVal,true);
		addMember(l,"StringVal",get_StringVal,set_StringVal,true);
		addMember(l,"IsNullObject",get_IsNullObject,null,true);
		addMember(l,"IsNullOrEmptyString",get_IsNullOrEmptyString,null,true);
		addMember(l,"IsObject",get_IsObject,null,true);
		addMember(l,"IsString",get_IsString,null,true);
		addMember(l,"IsBoolean",get_IsBoolean,null,true);
		addMember(l,"IsChar",get_IsChar,null,true);
		addMember(l,"IsInteger",get_IsInteger,null,true);
		addMember(l,"IsNumber",get_IsNumber,null,true);
		addMember(l,"AsString",get_AsString,null,true);
		addMember(l,"NullObject",get_NullObject,null,false);
		addMember(l,"EmptyString",get_EmptyString,null,false);
		createTypeMetatable(l,null, typeof(BoxedValue),typeof(System.ValueType));
	}
}
