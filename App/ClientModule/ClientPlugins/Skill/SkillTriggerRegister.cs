using UnityEngine;
using System.Collections.Generic;
using System.Text;
using GameFramework.Plugin;
using ClientPlugins;

public static class SkillTriggerRegister
{
    public static void Register()
    {
        Plugin.Proxy.RegisterSkillTrigger("trackbullet", "TrackBulletTrigger");
        Plugin.Proxy.RegisterSkillTrigger("track2", "Track2Trigger");
    }        
}
