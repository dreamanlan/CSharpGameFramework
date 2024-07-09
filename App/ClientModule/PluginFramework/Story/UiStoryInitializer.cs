using UnityEngine;
using System.Collections.Generic;
using ScriptableFramework;
using ScriptableFramework.Story;
using DotnetStoryScript;

namespace ScriptableFramework.Story
{
    public class UiStoryInitializer : MonoBehaviour
    {
        public string WindowName = string.Empty;
        public void Init()
        {
            StoryInstance instance = GfxStorySystem.Instance.GetStory("main", WindowName);
            if (null != instance) {
                instance.LocalVariables.Clear();
                instance.LocalVariables.Add("@window", BoxedValue.FromObject(gameObject));
                GfxStorySystem.Instance.StartStory("main", WindowName);
            }
        }
    }
}
