using UnityEngine;
using System.Collections;
using GameFramework.Story;

public class StoryMessage : MonoBehaviour
{
    public void SendStoryMessage(string msgId)
    {
        GfxStorySystem.Instance.SendMessage(msgId, gameObject.name);
    }
}
