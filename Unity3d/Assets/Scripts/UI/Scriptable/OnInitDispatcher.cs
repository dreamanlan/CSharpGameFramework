using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Story;
using StorySystem;

public class OnInitDispatcher : MonoBehaviour 
{
    [Serializable]
    public class ScriptableControl
    {
        public string Name;
        public GameObject Control;
    }
    public string WindowName = "";
    public ScriptableControl[] Controls = null;

	// Use this for initialization
	void Start () {
        StoryInstance instance = GfxStorySystem.Instance.GetStory("main", WindowName);
        if (null != instance) {
            instance.LocalVariables.Clear();
            instance.LocalVariables.Add("@window", gameObject);
            int ct = Controls.Length;
            for (int ix = 0; ix < ct; ++ix) {
                AddVariable(instance, Controls[ix].Name, Controls[ix].Control);
            }
            GfxStorySystem.Instance.StartStory("main", WindowName);
        }
	}

    private static void AddVariable(StoryInstance instance, string name, GameObject control)
    {
        instance.LocalVariables.Add(name, control);
        Text text = control.GetComponent<Text>();
        if (null != text) {
            instance.LocalVariables.Add(string.Format("{0}_Text", name), text);
        }
        Image image = control.GetComponent<Image>();
        if (null != image) {
            instance.LocalVariables.Add(string.Format("{0}_Image", name), image);
        }
        RawImage rawImage = control.GetComponent<RawImage>();
        if (null != rawImage) {
            instance.LocalVariables.Add(string.Format("{0}_RawImage", name), rawImage);
        }
        Button button = control.GetComponent<Button>();
        if (null != button) {
            instance.LocalVariables.Add(string.Format("{0}_Button", name), button);
        }
        Dropdown dropdown = control.GetComponent<Dropdown>();
        if (null != dropdown) {
            instance.LocalVariables.Add(string.Format("{0}_Dropdown", name), dropdown);
        }
        InputField inputField = control.GetComponent<InputField>();
        if (null != inputField) {
            instance.LocalVariables.Add(string.Format("{0}_Input", name), inputField);
        }
        Slider slider = control.GetComponent<Slider>();
        if (null != inputField) {
            instance.LocalVariables.Add(string.Format("{0}_Slider", name), slider);
        }
        Toggle toggle = control.GetComponent<Toggle>();
        if (null != toggle) {
            instance.LocalVariables.Add(string.Format("{0}_Toggle", name), toggle);
        }
        ToggleGroup toggleGroup = control.GetComponent<ToggleGroup>();
        if (null != toggleGroup) {
            instance.LocalVariables.Add(string.Format("{0}_ToggleGroup", name), toggleGroup);
        }
        Scrollbar scrollbar = control.GetComponent<Scrollbar>();
        if (null != scrollbar) {
            instance.LocalVariables.Add(string.Format("{0}_Scrollbar", name), scrollbar);
        }
    }
}
