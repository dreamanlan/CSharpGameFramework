using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RenderCubemap : ScriptableWizard
{
    public Transform renderTrans;
    public Cubemap cubemap;

    [MenuItem("Tools/CreateCubemap")]
    static void CreateCubemap()
    {
        ScriptableWizard.DisplayWizard<RenderCubemap>("Create Cubemap", "Create");
    }

    private void OnWizardCreate()
    {
        var go = new GameObject();
        go.transform.position = renderTrans.position;
        var camera = go.AddComponent<Camera>();
        camera.RenderToCubemap(cubemap);
        DestroyImmediate(go);
    }

    private void OnWizardUpdate()
    {
        helpString = "选择渲染位置并确定需设置的Cubemap";
        isValid = null != renderTrans && null != cubemap;
    }
}
