using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using SLua;

internal class Cs2LuaAssembly
{
    internal bool LuaInited
    {
        get { return m_LuaInited; }
    }
    internal Assembly Assembly
    {
        get { return m_Assembly; }
    }
    public LuaSvr LuaSvr
    {
        get { return m_LuaSvr; }
    }
    internal void Init()
    {
#if !CS2LUA_DEBUG
        ObjectCache.AddAQName(typeof(LinkedListNode<GameFramework.EntityInfo>), "LinkedListNodeEntityInfo");
        ObjectCache.AddAQName(typeof(KeyValuePair<int, int>), "IntIntPair");
        ObjectCache.AddAQName(typeof(KeyValuePair<int, float>), "IntFloatPair");
        ObjectCache.AddAQName(typeof(KeyValuePair<int, string>), "IntStrPair");
        ObjectCache.AddAQName(typeof(KeyValuePair<int, object>), "IntObjPair");
        ObjectCache.AddAQName(typeof(KeyValuePair<int, UnityEngine.Object>), "IntUobjPair");
        ObjectCache.AddAQName(typeof(KeyValuePair<string, int>), "StrIntPair");
        ObjectCache.AddAQName(typeof(KeyValuePair<string, float>), "StrFloatPair");
        ObjectCache.AddAQName(typeof(KeyValuePair<string, string>), "StrStrPair");
        ObjectCache.AddAQName(typeof(KeyValuePair<string, object>), "StrObjPair");
        ObjectCache.AddAQName(typeof(KeyValuePair<string, UnityEngine.Object>), "StrUobjPair");
        m_LuaSvr.init(null, () => {
            var entry = (LuaTable)m_LuaSvr.start("Cs2LuaScript__Program");
            entry.invoke("Init");
            m_LuaInited = true;
        });
#endif

#if CS2LUA_DEBUG || UNITY_IOS
        Cs2LuaScript.Program.Init();
#else
        Load(LoadFileFromStreamingAssets("Cs2LuaScript.dll"));
        if (null != m_Assembly) {
            var type = m_Assembly.GetType("Cs2LuaScript.Program");
            type.InvokeMember("Init", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, null);
        }
#endif
    }
    internal void Load(byte[] bytes)
    {
        m_Assembly = Assembly.Load(bytes);
    }

    private Assembly m_Assembly = null;
#if CS2LUA_DEBUG
    private LuaSvr m_LuaSvr = null;    
#else
    private LuaSvr m_LuaSvr = new LuaSvr();
#endif
    private bool m_LuaInited = false;
    
    internal static Cs2LuaAssembly Instance
    {
        get { return s_Instance; }
    }
    private static Cs2LuaAssembly s_Instance = new Cs2LuaAssembly();

    private static byte[] LoadFileFromStreamingAssets(string file)
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        string assembly = Path.Combine(Application.streamingAssetsPath, file);
        if (File.Exists(assembly)) {
            var bytes = File.ReadAllBytes(assembly);
            return bytes;
        }
#elif UNITY_ANDROID
        AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject androidJavaActivity = null;
        AndroidJavaObject assetManager = null;
        AndroidJavaObject inputStream = null;
        if (androidJavaClass != null)
            androidJavaActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
        if (androidJavaActivity != null)
            assetManager = androidJavaActivity.Call<AndroidJavaObject>("getAssets");
        if (assetManager != null)
            inputStream = assetManager.Call<AndroidJavaObject>("open", file);
        if (inputStream != null) {
            int available = inputStream.Call<int>("available");
            System.IntPtr buffer = AndroidJNI.NewByteArray(available);
            System.IntPtr javaClass = AndroidJNI.FindClass("java/io/InputStream");
            System.IntPtr javaMethodID = AndroidJNIHelper.GetMethodID(javaClass, "read", "([B)I");
            int read = AndroidJNI.CallIntMethod(inputStream.GetRawObject(), javaMethodID,
                new[] { new jvalue() { l = buffer } });
            byte[] bytes = AndroidJNI.FromByteArray(buffer);
            AndroidJNI.DeleteLocalRef(buffer);
            inputStream.Call("close");
            inputStream.Dispose();
            return bytes;
        }

#endif
        return null;
    }
}
