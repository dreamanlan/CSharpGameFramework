using UnityEngine;
//using UnityEngine.UI;
using UnityEditor;
//using UnityEditor.UI;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEditor.MemoryProfiler;
using UnityEditorInternal;
using UnityEditor.Profiling;
using UnityEditor.Profiling.Memory.Experimental;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using StoryScript;
using StoryScript.DslExpression;

public sealed class AppEditorDslApi
{
    [InitializeOnLoadMethod]
    public static void RunOnEditorStartup()
    {
        EditorDslProcessorUtility.OnRegisterApi = Register;
        EditorGmCommandWindow.RunOnEditorStartup();
    }
    private static void Register(DslCalculator calc)
    {
    }
}
