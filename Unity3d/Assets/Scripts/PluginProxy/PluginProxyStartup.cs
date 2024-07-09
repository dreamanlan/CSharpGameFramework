using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ScriptableFramework.Plugin;

public class PluginProxyStartup : MonoBehaviour
{
    public PluginType PluginType;
    public string ScriptClassFileName;

    internal void Start()
    {
        string className = ScriptClassFileName.Replace("__",".");
        if (PluginType == PluginType.Script) {
            scriptInited = false;
            StartCoroutine(StartupScript(className));
        } else {
            monoBehaviourProxy = new MonoBehaviourProxy(this);
            csObject = PluginManager.Instance.CreateStartup(className);
            csObject.Init(gameObject, monoBehaviourProxy);
        }
    }

    private IEnumerator StartupScript(string className)
    {
        while (!PluginAssembly.Instance.ScriptInited)
            yield return null;
		string fileName = ScriptClassFileName.ToLower();
        scriptInited = true;
        yield return null;
    }

    private void CallScript(object[] args)
    {
        if (args.Length > 0) {
            if (PluginType == PluginType.Script) {
            } else {
                if (null != csObject) {
                    string name = args[0] as string;
                    ArrayList arr = new ArrayList(args);
                    arr.RemoveAt(0);
                    if (args.Length == 1)
                        csObject.Call(name);
                    else if (args.Length == 2)
                        csObject.Call(name, args[1]);
                    else
                        csObject.Call(name, arr.ToArray());
                }
            }
        }
    }

    private IStartupPlugin csObject;
    
    private bool scriptInited;
    private MonoBehaviourProxy monoBehaviourProxy;
}
