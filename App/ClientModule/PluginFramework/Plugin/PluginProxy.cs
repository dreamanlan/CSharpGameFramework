using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptableFramework
{
    public interface IPluginProxy
    {
        void RegisterAttrExpression(string name, string implClass);
        void RegisterSkillTrigger(string name, string implClass);
        void RegisterStoryApi(string name, string doc, string implClass);
        void RegisterSimpleStoryApi(string name, string doc, string implClass);
        void InstallStartupPlugin(string objpath, string implClass);
        void RemoveStartupPlugin(string objpath, string implClass);
        void InstallTickPlugin(string objpath, string implClass);
        void RemoveTickPlugin(string objpath, string implClass);
    }
    public static class PluginProxy
    {
        public static IPluginProxy NativeProxy
        {
            get { return m_NativeProxy; }
            set { m_NativeProxy = value; }
        }
        public static IPluginProxy ScriptProxy
        {
            get { return m_ScriptProxy; }
            set { m_ScriptProxy = value; }
        }

        private static IPluginProxy m_NativeProxy = null;
        private static IPluginProxy m_ScriptProxy = null;
    }
}
