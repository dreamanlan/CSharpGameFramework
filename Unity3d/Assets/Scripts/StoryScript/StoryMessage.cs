using UnityEngine;
using System.Collections;
using ScriptableFramework.Story;

public class StoryMessage : MonoBehaviour
{
    public void SendStoryMessage(string msgId)
    {
        GfxStorySystem.Instance.SendMessage(msgId, gameObject.name);
    }
}
