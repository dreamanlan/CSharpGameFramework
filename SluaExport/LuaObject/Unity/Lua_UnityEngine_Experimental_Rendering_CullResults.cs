using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_Experimental_Rendering_CullResults : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.Experimental.Rendering.CullResults o;
			o=new UnityEngine.Experimental.Rendering.CullResults();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetShadowCasterBounds(IntPtr l) {
		try {
			UnityEngine.Experimental.Rendering.CullResults self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			UnityEngine.Bounds a2;
			var ret=self.GetShadowCasterBounds(a1,out a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetLightIndicesCount(IntPtr l) {
		try {
			UnityEngine.Experimental.Rendering.CullResults self;
			checkValueType(l,1,out self);
			var ret=self.GetLightIndicesCount();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FillLightIndices(IntPtr l) {
		try {
			UnityEngine.Experimental.Rendering.CullResults self;
			checkValueType(l,1,out self);
			UnityEngine.ComputeBuffer a1;
			checkType(l,2,out a1);
			self.FillLightIndices(a1);
			pushValue(l,true);
			setBack(l,self);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ComputeSpotShadowMatricesAndCullingPrimitives(IntPtr l) {
		try {
			UnityEngine.Experimental.Rendering.CullResults self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			UnityEngine.Matrix4x4 a2;
			UnityEngine.Matrix4x4 a3;
			UnityEngine.Experimental.Rendering.ShadowSplitData a4;
			var ret=self.ComputeSpotShadowMatricesAndCullingPrimitives(a1,out a2,out a3,out a4);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			pushValue(l,a3);
			pushValue(l,a4);
			return 5;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ComputePointShadowMatricesAndCullingPrimitives(IntPtr l) {
		try {
			UnityEngine.Experimental.Rendering.CullResults self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			UnityEngine.CubemapFace a2;
			checkEnum(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			UnityEngine.Matrix4x4 a4;
			UnityEngine.Matrix4x4 a5;
			UnityEngine.Experimental.Rendering.ShadowSplitData a6;
			var ret=self.ComputePointShadowMatricesAndCullingPrimitives(a1,a2,a3,out a4,out a5,out a6);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a4);
			pushValue(l,a5);
			pushValue(l,a6);
			return 5;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ComputeDirectionalShadowMatricesAndCullingPrimitives(IntPtr l) {
		try {
			UnityEngine.Experimental.Rendering.CullResults self;
			checkValueType(l,1,out self);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			UnityEngine.Vector3 a4;
			checkType(l,5,out a4);
			System.Int32 a5;
			checkType(l,6,out a5);
			System.Single a6;
			checkType(l,7,out a6);
			UnityEngine.Matrix4x4 a7;
			UnityEngine.Matrix4x4 a8;
			UnityEngine.Experimental.Rendering.ShadowSplitData a9;
			var ret=self.ComputeDirectionalShadowMatricesAndCullingPrimitives(a1,a2,a3,a4,a5,a6,out a7,out a8,out a9);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a7);
			pushValue(l,a8);
			pushValue(l,a9);
			return 5;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetCullingParameters_s(IntPtr l) {
		try {
			UnityEngine.Camera a1;
			checkType(l,1,out a1);
			UnityEngine.Experimental.Rendering.CullingParameters a2;
			var ret=UnityEngine.Experimental.Rendering.CullResults.GetCullingParameters(a1,out a2);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Cull_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UnityEngine.Experimental.Rendering.CullingParameters a1;
				checkValueType(l,1,out a1);
				UnityEngine.Experimental.Rendering.ScriptableRenderContext a2;
				checkValueType(l,2,out a2);
				var ret=UnityEngine.Experimental.Rendering.CullResults.Cull(ref a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a1);
				return 3;
			}
			else if(argc==3){
				UnityEngine.Camera a1;
				checkType(l,1,out a1);
				UnityEngine.Experimental.Rendering.ScriptableRenderContext a2;
				checkValueType(l,2,out a2);
				UnityEngine.Experimental.Rendering.CullResults a3;
				var ret=UnityEngine.Experimental.Rendering.CullResults.Cull(a1,a2,out a3);
				pushValue(l,true);
				pushValue(l,ret);
				pushValue(l,a3);
				return 3;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_visibleLights(IntPtr l) {
		try {
			UnityEngine.Experimental.Rendering.CullResults self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.visibleLights);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_visibleLights(IntPtr l) {
		try {
			UnityEngine.Experimental.Rendering.CullResults self;
			checkValueType(l,1,out self);
			UnityEngine.Experimental.Rendering.VisibleLight[] v;
			checkArray(l,2,out v);
			self.visibleLights=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_visibleReflectionProbes(IntPtr l) {
		try {
			UnityEngine.Experimental.Rendering.CullResults self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.visibleReflectionProbes);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_visibleReflectionProbes(IntPtr l) {
		try {
			UnityEngine.Experimental.Rendering.CullResults self;
			checkValueType(l,1,out self);
			UnityEngine.Experimental.Rendering.VisibleReflectionProbe[] v;
			checkArray(l,2,out v);
			self.visibleReflectionProbes=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.Experimental.Rendering.CullResults");
		addMember(l,GetShadowCasterBounds);
		addMember(l,GetLightIndicesCount);
		addMember(l,FillLightIndices);
		addMember(l,ComputeSpotShadowMatricesAndCullingPrimitives);
		addMember(l,ComputePointShadowMatricesAndCullingPrimitives);
		addMember(l,ComputeDirectionalShadowMatricesAndCullingPrimitives);
		addMember(l,GetCullingParameters_s);
		addMember(l,Cull_s);
		addMember(l,"visibleLights",get_visibleLights,set_visibleLights,true);
		addMember(l,"visibleReflectionProbes",get_visibleReflectionProbes,set_visibleReflectionProbes,true);
		createTypeMetatable(l,constructor, typeof(UnityEngine.Experimental.Rendering.CullResults),typeof(System.ValueType));
	}
}
