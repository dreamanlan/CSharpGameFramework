using System;
using System.Collections.Generic;

using System.Text;

namespace DotnetSkillScript
{
    /// <summary>
    /// This class does not lock, constraint: the registration of all triggers must be completed when the program starts.
    /// </summary>
    public sealed class SkillTrigerManager
    {
        public void RegisterTrigerFactory(string type, ISkillTrigerFactory factory)
        {
            RegisterTrigerFactory(type, factory, false);
        }
        public void RegisterTrigerFactory(string type, ISkillTrigerFactory factory, bool replace)
        {
            if (!m_TrigerFactories.ContainsKey(type)) {
                m_TrigerFactories.Add(type, factory);
            } else if(replace) {
                //Allow trigger reloading
                m_TrigerFactories[type] = factory;
            } else {
                //error
            }
        }
        public ISkillTriger CreateTriger(Dsl.ISyntaxComponent trigerConfig, SkillInstance instance)
        {
            ISkillTriger triger = null;
            string type = trigerConfig.GetId();
            ISkillTrigerFactory factory = GetFactory(type);
            if (null != factory) {
                try {
                    triger = factory.Create();
                    triger.Init(trigerConfig, instance);
                } catch (Exception ex) {
                    ScriptableFramework.LogSystem.Error("triger:{0} line:{1} failed.", trigerConfig.ToScriptString(false, Dsl.DelimiterInfo.Default), trigerConfig.GetLine());
                    throw ex;
                }
            } else {
#if !DEBUG
                ScriptableFramework.LogSystem.Error("CreateTriger failed, type:{0}", type);
#endif
            }
            if (null != triger) {
                ScriptableFramework.LogSystem.Debug("CreateTriger, type:{0} triger:{1}", type, triger.GetType().Name);
            } else {
#if !DEBUG
                ScriptableFramework.LogSystem.Error("CreateTriger failed, type:{0}", type);
#endif
            }
            return triger;
        }

        private ISkillTrigerFactory GetFactory(string type)
        {
            ISkillTrigerFactory factory;
            lock (m_Lock) {
                m_TrigerFactories.TryGetValue(type, out factory);
            }
            return factory;
        }

        private SkillTrigerManager() { }

        private object m_Lock = new object();
        private Dictionary<string, ISkillTrigerFactory> m_TrigerFactories = new Dictionary<string, ISkillTrigerFactory>();

        public static SkillTrigerManager Instance
        {
            get { return s_Instance; }
        }
        private static SkillTrigerManager s_Instance = new SkillTrigerManager();
    }
}
