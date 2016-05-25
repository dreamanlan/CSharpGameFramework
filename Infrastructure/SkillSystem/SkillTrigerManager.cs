using System;
using System.Collections.Generic;

using System.Text;

namespace SkillSystem
{
    /// <summary>
    /// 这个类不加锁，约束条件：所有触发器的注册必须在程序启动时完成。
    /// </summary>
    public sealed class SkillTrigerManager
    {
        public void RegisterTrigerFactory(string type, ISkillTrigerFactory factory)
        {
            if (!m_TrigerFactories.ContainsKey(type)) {
                m_TrigerFactories.Add(type, factory);
            } else {
                //error
            }
        }
        public ISkillTriger CreateTriger(Dsl.ISyntaxComponent trigerConfig, int dslSkillId)
        {
            ISkillTriger triger = null;
            string type = trigerConfig.GetId();
            ISkillTrigerFactory factory = GetFactory(type);
            if (null != factory) {
                try {
                    triger = factory.Create(trigerConfig, dslSkillId);
                } catch (Exception ex) {
                    GameFramework.LogSystem.Error("triger:{0} line:{1} failed.", trigerConfig.ToScriptString(), trigerConfig.GetLine());
                    throw ex;
                }
            } else {
#if !DEBUG
                CsLibrary.LogSystem.Error("CreateTriger failed, type:{0}", type);
#endif
            }
            if (null != triger) {
                GameFramework.LogSystem.Debug("CreateTriger, type:{0} triger:{1}", type, triger.GetType().Name);
            } else {
#if !DEBUG
                CsLibrary.LogSystem.Error("CreateTriger failed, type:{0}", type);
#endif
            }
            return triger;
        }

        private ISkillTrigerFactory GetFactory(string type)
        {
            ISkillTrigerFactory factory;
            m_TrigerFactories.TryGetValue(type, out factory);
            return factory;
        }

        private SkillTrigerManager() { }

        private Dictionary<string, ISkillTrigerFactory> m_TrigerFactories = new Dictionary<string, ISkillTrigerFactory>();

        public static SkillTrigerManager Instance
        {
            get { return s_Instance; }
        }
        private static SkillTrigerManager s_Instance = new SkillTrigerManager();
    }
}
