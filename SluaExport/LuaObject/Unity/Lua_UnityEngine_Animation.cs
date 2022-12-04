﻿using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_UnityEngine_Animation : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Stop(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			self.Stop();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Stop__String(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Stop(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Rewind(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			self.Rewind();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Rewind__String(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Rewind(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Sample(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			self.Sample();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int IsPlaying(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.IsPlaying(a1);
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
	static public int Play(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			var ret=self.Play();
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
	static public int Play__PlayMode(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			UnityEngine.PlayMode a1;
			checkEnum(l,2,out a1);
			var ret=self.Play(a1);
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
	static public int Play__String(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.Play(a1);
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
	static public int Play__String__PlayMode(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			UnityEngine.PlayMode a2;
			checkEnum(l,3,out a2);
			var ret=self.Play(a1,a2);
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
	static public int CrossFade__String(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.CrossFade(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CrossFade__String__Single(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			self.CrossFade(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CrossFade__String__Single__PlayMode(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			UnityEngine.PlayMode a3;
			checkEnum(l,4,out a3);
			self.CrossFade(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Blend__String(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Blend(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Blend__String__Single(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			self.Blend(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int Blend__String__Single__Single(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			System.Single a3;
			checkType(l,4,out a3);
			self.Blend(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int CrossFadeQueued__String(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.CrossFadeQueued(a1);
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
	static public int CrossFadeQueued__String__Single(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			var ret=self.CrossFadeQueued(a1,a2);
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
	static public int CrossFadeQueued__String__Single__QueueMode(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			UnityEngine.QueueMode a3;
			checkEnum(l,4,out a3);
			var ret=self.CrossFadeQueued(a1,a2,a3);
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
	static public int CrossFadeQueued__String__Single__QueueMode__PlayMode(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			UnityEngine.QueueMode a3;
			checkEnum(l,4,out a3);
			UnityEngine.PlayMode a4;
			checkEnum(l,5,out a4);
			var ret=self.CrossFadeQueued(a1,a2,a3,a4);
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
	static public int PlayQueued__String(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.PlayQueued(a1);
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
	static public int PlayQueued__String__QueueMode(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			UnityEngine.QueueMode a2;
			checkEnum(l,3,out a2);
			var ret=self.PlayQueued(a1,a2);
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
	static public int PlayQueued__String__QueueMode__PlayMode(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			UnityEngine.QueueMode a2;
			checkEnum(l,3,out a2);
			UnityEngine.PlayMode a3;
			checkEnum(l,4,out a3);
			var ret=self.PlayQueued(a1,a2,a3);
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
	static public int AddClip__AnimationClip__String(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			UnityEngine.AnimationClip a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			self.AddClip(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddClip__AnimationClip__String__Int32__Int32(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			UnityEngine.AnimationClip a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Int32 a4;
			checkType(l,5,out a4);
			self.AddClip(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int AddClip__AnimationClip__String__Int32__Int32__Boolean(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			UnityEngine.AnimationClip a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			System.Int32 a3;
			checkType(l,4,out a3);
			System.Int32 a4;
			checkType(l,5,out a4);
			System.Boolean a5;
			checkType(l,6,out a5);
			self.AddClip(a1,a2,a3,a4,a5);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveClip__AnimationClip(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			UnityEngine.AnimationClip a1;
			checkType(l,2,out a1);
			self.RemoveClip(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int RemoveClip__String(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.RemoveClip(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetClipCount(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			var ret=self.GetClipCount();
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
	static public int SyncLayer(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.SyncLayer(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int GetEnumerator(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			var ret=self.GetEnumerator();
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
	static public int GetClip(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			var ret=self.GetClip(a1);
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
	static public int get_clip(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.clip);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_clip(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			UnityEngine.AnimationClip v;
			checkType(l,2,out v);
			self.clip=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_playAutomatically(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.playAutomatically);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_playAutomatically(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.playAutomatically=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_wrapMode(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.wrapMode);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_wrapMode(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			UnityEngine.WrapMode v;
			checkEnum(l,2,out v);
			self.wrapMode=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_isPlaying(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.isPlaying);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_animatePhysics(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.animatePhysics);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_animatePhysics(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.animatePhysics=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_cullingType(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.cullingType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_cullingType(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			UnityEngine.AnimationCullingType v;
			checkEnum(l,2,out v);
			self.cullingType=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int get_localBounds(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.localBounds);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int set_localBounds(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			UnityEngine.Bounds v;
			checkValueType(l,2,out v);
			self.localBounds=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	[UnityEngine.Scripting.Preserve]
	static public int getItem(IntPtr l) {
		try {
			UnityEngine.Animation self=(UnityEngine.Animation)checkSelf(l);
			string v;
			checkType(l,2,out v);
			var ret = self[v];
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
		getTypeTable(l,"UnityEngine.Animation");
		addMember(l,Stop);
		addMember(l,Stop__String);
		addMember(l,Rewind);
		addMember(l,Rewind__String);
		addMember(l,Sample);
		addMember(l,IsPlaying);
		addMember(l,Play);
		addMember(l,Play__PlayMode);
		addMember(l,Play__String);
		addMember(l,Play__String__PlayMode);
		addMember(l,CrossFade__String);
		addMember(l,CrossFade__String__Single);
		addMember(l,CrossFade__String__Single__PlayMode);
		addMember(l,Blend__String);
		addMember(l,Blend__String__Single);
		addMember(l,Blend__String__Single__Single);
		addMember(l,CrossFadeQueued__String);
		addMember(l,CrossFadeQueued__String__Single);
		addMember(l,CrossFadeQueued__String__Single__QueueMode);
		addMember(l,CrossFadeQueued__String__Single__QueueMode__PlayMode);
		addMember(l,PlayQueued__String);
		addMember(l,PlayQueued__String__QueueMode);
		addMember(l,PlayQueued__String__QueueMode__PlayMode);
		addMember(l,AddClip__AnimationClip__String);
		addMember(l,AddClip__AnimationClip__String__Int32__Int32);
		addMember(l,AddClip__AnimationClip__String__Int32__Int32__Boolean);
		addMember(l,RemoveClip__AnimationClip);
		addMember(l,RemoveClip__String);
		addMember(l,GetClipCount);
		addMember(l,SyncLayer);
		addMember(l,GetEnumerator);
		addMember(l,GetClip);
		addMember(l,getItem);
		addMember(l,"clip",get_clip,set_clip,true);
		addMember(l,"playAutomatically",get_playAutomatically,set_playAutomatically,true);
		addMember(l,"wrapMode",get_wrapMode,set_wrapMode,true);
		addMember(l,"isPlaying",get_isPlaying,null,true);
		addMember(l,"animatePhysics",get_animatePhysics,set_animatePhysics,true);
		addMember(l,"cullingType",get_cullingType,set_cullingType,true);
		addMember(l,"localBounds",get_localBounds,set_localBounds,true);
		createTypeMetatable(l,null, typeof(UnityEngine.Animation),typeof(UnityEngine.Behaviour));
	}
}
