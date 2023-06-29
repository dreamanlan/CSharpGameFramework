using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;

public class ScriptSvr
{

}

internal class PluginAssembly
{
    internal bool ScriptInited
    {
        get { return m_ScriptInited; }
    }
    internal Assembly Assembly
    {
        get { return m_Assembly; }
    }
    public ScriptSvr ScriptSvr
    {
        get { return m_ScriptSvr; }
    }
    internal void Init()
    {
#if CS2LUA_DEBUG || UNITY_IOS
        ClientPlugins.Program.Init();
#else
        Load(LoadFileFromStreamingAssets("ClientPlugins.dll"));
        if (null != m_Assembly) {
            var type = m_Assembly.GetType("ClientPlugins.Program");
            type.InvokeMember("Init", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, null);
        }
#endif
    }
    internal void Load(byte[] bytes)
    {
        m_Assembly = Assembly.Load(bytes);
    }

    private Assembly m_Assembly = null;
    private ScriptSvr m_ScriptSvr = null;
    private bool m_ScriptInited = false;
    
    internal static PluginAssembly Instance
    {
        get { return s_Instance; }
    }
    private static PluginAssembly s_Instance = new PluginAssembly();

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
