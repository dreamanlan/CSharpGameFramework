using System;
using System.Collections.Generic;
using GameFramework;

namespace SkillSystem
{
    public sealed class SkillConfigManager
    {
        public void LoadSkillIfNotExist(int id, string file)
        {
            if (!ExistSkill(id)) {
                LoadSkill(file);
            }
        }
        public bool ExistSkill(int id)
        {
            return null != GetSkillInstanceResource(id);
        }
        public void LoadSkill(string file)
        {
            if (!string.IsNullOrEmpty(file)) {
                Dsl.DslFile dataFile = new Dsl.DslFile();
#if DEBUG
                try {
                    if (dataFile.Load(file, LogSystem.Log)) {
                        Load(dataFile);
                    } else {
                        LogSystem.Error("LoadSkill file:{0} failed", file);
                    }
                } catch (Exception ex) {
                    LogSystem.Error("LoadSkill file:{0} Exception:{1}\n{2}", file, ex.Message, ex.StackTrace);
                }
#else
        try {
          dataFile.LoadObfuscatedFile(file, GlobalVariables.Instance.DecodeTable);
          Load(dataFile);
        } catch {
        }
#endif
            }
        }
        public void LoadSkillText(string text)
        {
            Dsl.DslFile dataFile = new Dsl.DslFile();
#if DEBUG
            try {
                if (dataFile.LoadFromString(text, "skill", LogSystem.Log)) {
                    Load(dataFile);
                } else {
                    LogSystem.Error("LoadSkillText text:{0} failed", text);
                }
            } catch (Exception ex) {
                LogSystem.Error("LoadSkillText text:{0} Exception:{1}\n{2}", text, ex.Message, ex.StackTrace);
            }
#else
      try {
        dataFile.LoadObfuscatedCode(text, GlobalVariables.Instance.DecodeTable);
        Load(dataFile);
      } catch {
      }
#endif
        }
        public SkillInstance NewSkillInstance(int id)
        {
            SkillInstance instance = null;
            SkillInstance temp = GetSkillInstanceResource(id);
            if (null != temp) {
                instance = temp.Clone();
            }
            return instance;
        }
        public void Clear()
        {
            lock (m_Lock) {
                m_SkillInstances.Clear();
            }
        }

        private void Load(Dsl.DslFile dataFile)
        {
            lock (m_Lock) {
                for (int i = 0; i < dataFile.DslInfos.Count; i++) {
                    if (dataFile.DslInfos[i].GetId() == "skill") {
                        Dsl.FunctionData funcData = dataFile.DslInfos[i].First;
                        if (null != funcData) {
                            Dsl.CallData callData = funcData.Call;
                            if (null != callData && callData.HaveParam()) {
                                int id = int.Parse(callData.GetParamId(0));
                                if (!m_SkillInstances.ContainsKey(id)) {
                                    SkillInstance instance = new SkillInstance();
                                    instance.Init(dataFile.DslInfos[i]);
                                    m_SkillInstances.Add(id, instance);

                                    LogSystem.Debug("ParseSkill {0}", id);
                                }
                                //else
                                //{
                                //repeated skill config.
                                //}
                            }
                        }
                    }
                }
            }
        }
        private SkillInstance GetSkillInstanceResource(int id)
        {
            SkillInstance instance = null;
            lock (m_Lock) {
                m_SkillInstances.TryGetValue(id, out instance);
            }
            return instance;
        }

        private SkillConfigManager() { }

        private object m_Lock = new object();
        private Dictionary<int, SkillInstance> m_SkillInstances = new Dictionary<int, SkillInstance>();

        public static SkillConfigManager Instance
        {
            get { return s_Instance; }
        }
        private static SkillConfigManager s_Instance = new SkillConfigManager();
    }
}
