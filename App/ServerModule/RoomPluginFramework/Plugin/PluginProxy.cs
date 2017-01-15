using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework.Plugin
{
    public interface IPluginProxy
    {
        void RegisterSkillTrigger(string name, string implClass);
        void RegisterStoryCommand(string name, string implClass);
        void RegisterStoryValue(string name, string implClass);
        void RegisterSimpleStoryCommand(string name, string implClass);
        void RegisterSimpleStoryValue(string name, string implClass);
    }
    public static class PluginProxy
    {
        public static IPluginProxy NativeProxy
        {
            get { return m_NativeProxy; }
            set { m_NativeProxy = value; }
        }
        public static IPluginProxy LuaProxy
        {
            get { return m_LuaProxy; }
            set { m_LuaProxy = value; }
        }

        private static IPluginProxy m_NativeProxy = null;
        private static IPluginProxy m_LuaProxy = null;
    }
}
