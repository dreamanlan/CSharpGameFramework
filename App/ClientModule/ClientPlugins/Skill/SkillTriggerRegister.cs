using UnityEngine;
using System.Collections.Generic;
using System.Text;
using ScriptableFramework;
using ClientPlugins;

public static class SkillTriggerRegister
{
    public static void Register()
    {
        Plugin.Proxy.RegisterSkillTrigger("track_bullet", "TrackBulletTrigger");
        Plugin.Proxy.RegisterSkillTrigger("track_2", "Track2Trigger");
    }        
}
