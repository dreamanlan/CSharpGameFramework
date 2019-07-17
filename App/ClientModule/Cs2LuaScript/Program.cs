using System;
using System.Collections.Generic;
using System.Text;
using GameFramework.Plugin;

namespace Cs2LuaScript
{
    public static class Plugin
    {
#if __LUA__
        public static IPluginProxy Proxy
        {
            get
            {
                return PluginProxy.LuaProxy;
            }
        }
#else
        public static IPluginProxy Proxy
        {
            get
            {
                return PluginProxy.NativeProxy;
            }
        }
#endif
    }
    [Cs2Dsl.Entry]
    public static class Program
    {
        [Cs2Dsl.Ignore]
        public static void InitDll()
        {
            PluginManager.Instance.RegisterStartupFactory("Startup", new StartupPluginFactory<Startup>());
            PluginManager.Instance.RegisterTickFactory("UiScrollInfo", new TickPluginFactory<UiScrollInfo>());
            PluginManager.Instance.RegisterTickFactory("MiniMap", new TickPluginFactory<MiniMap>());

            PluginManager.Instance.RegisterObjectFactory("TrackBulletTrigger", new ObjectPluginFactory<TrackBulletTrigger>());
            PluginManager.Instance.RegisterObjectFactory("Track2Trigger", new ObjectPluginFactory<Track2Trigger>());

            PluginManager.Instance.RegisterObjectFactory("AiDoNormal", new ObjectPluginFactory<AiDoNormal>());
            PluginManager.Instance.RegisterObjectFactory("AiDoMember", new ObjectPluginFactory<AiDoMember>());
            PluginManager.Instance.RegisterObjectFactory("AiCastSkill", new ObjectPluginFactory<AiCastSkill>());
            PluginManager.Instance.RegisterObjectFactory("AiChase", new ObjectPluginFactory<AiChase>());
            PluginManager.Instance.RegisterObjectFactory("AiKeepAway", new ObjectPluginFactory<AiKeepAway>());
            PluginManager.Instance.RegisterObjectFactory("AiGohome", new ObjectPluginFactory<AiGohome>());
            PluginManager.Instance.RegisterObjectFactory("AiRandMove", new ObjectPluginFactory<AiRandMove>());

            PluginManager.Instance.RegisterObjectFactory("AiQuery", new ObjectPluginFactory<AiQuery>());
            PluginManager.Instance.RegisterObjectFactory("AiGetEntities", new ObjectPluginFactory<AiGetEntities>());
            PluginManager.Instance.RegisterObjectFactory("AiGetSkills", new ObjectPluginFactory<AiGetSkills>());
            PluginManager.Instance.RegisterObjectFactory("AiGetSkill", new ObjectPluginFactory<AiGetSkill>());
            PluginManager.Instance.RegisterObjectFactory("AiGetTarget", new ObjectPluginFactory<AiGetTarget>());
            PluginManager.Instance.RegisterObjectFactory("AiSelectSkill", new ObjectPluginFactory<AiSelectSkill>());
            PluginManager.Instance.RegisterObjectFactory("AiNeedChase", new ObjectPluginFactory<AiNeedChase>());
            PluginManager.Instance.RegisterObjectFactory("AiNeedKeepAway", new ObjectPluginFactory<AiNeedKeepAway>());
            PluginManager.Instance.RegisterObjectFactory("AiSelectSkillByDistance", new ObjectPluginFactory<AiSelectSkillByDistance>());
            PluginManager.Instance.RegisterObjectFactory("AiSelectTarget", new ObjectPluginFactory<AiSelectTarget>());
        }
        public static void Init()
        {
            //使用c#代码时需要的初始化（平台相关代码，不会转换为lua代码，cs2lua在进行翻译时会添加宏__LUA__）
#if __LUA__

#else
            InitDll();
#endif
            //公共初始化（也就是逻辑相关的代码）
        }
        private static void Main(string[] args)
        {
            Init();
        }
    }
}
