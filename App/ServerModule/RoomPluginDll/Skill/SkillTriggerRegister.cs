using System;
using System.Collections.Generic;
using System.Text;
using GameFramework.Plugin;
using Cs2LuaScript;

public static class SkillTriggerRegister
{
    public static void Register()
    {
        Plugin.Proxy.RegisterSkillTrigger("track2", "Track2Trigger");
    }        
}
