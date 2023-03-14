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
                LoadSkill(id, file);
            }
        }
        public bool ExistSkill(int id)
        {
            return null != GetSkillInstanceResource(id);
        }
        public void LoadSkill(int id, string file)
        {
            if (!string.IsNullOrEmpty(file)) {
                Dsl.DslFile dataFile = new Dsl.DslFile();
#if DEBUG
                try {
                    if (dataFile.Load(file, LogSystem.Log)) {
                        Load(id, dataFile);
                    } else {
                        LogSystem.Error("LoadSkill file:{0} failed", file);
                    }
                } catch (Exception ex) {
                    LogSystem.Error("LoadSkill file:{0} Exception:{1}\n{2}", file, ex.Message, ex.StackTrace);
                }
#else
        try {
          dataFile.LoadBinaryFile(file);
          Load(id, dataFile);
        } catch {
        }
#endif
            }
        }
        public void LoadSkillText(int id, string text)
        {
            Dsl.DslFile dataFile = new Dsl.DslFile();
#if DEBUG
            try {
                if (dataFile.LoadFromString(text, LogSystem.Log)) {
                    Load(id, dataFile);
                } else {
                    LogSystem.Error("LoadSkillText text:{0} failed", text);
                }
            } catch (Exception ex) {
                LogSystem.Error("LoadSkillText text:{0} Exception:{1}\n{2}", text, ex.Message, ex.StackTrace);
            }
#else
      try {
        dataFile.LoadBinaryCode(System.Text.Encoding.UTF8.GetBytes(text));
        Load(id, dataFile);
      } catch {
      }
#endif
        }
        public void LoadSkillDsl(int id, Dsl.FunctionData funcData)
        {
            lock (m_Lock) {
                Load(id, funcData);
            }
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

        private void Load(int id, Dsl.DslFile dataFile)
        {
            lock (m_Lock) {
                for (int i = 0; i < dataFile.DslInfos.Count; i++) {
                    var info = dataFile.DslInfos[i];
                    var func = info as Dsl.FunctionData;
                    var stData = info as Dsl.StatementData;
                    if (null == func && null != stData) {
                        func = stData.First.AsFunction;
                    }
                    if (null == func)
                        continue;
                    Load(id, func);
                }
            }
        }
        private void Load(int id, Dsl.FunctionData funcData)
        {
            if (null != funcData && funcData.IsHighOrder) {
                string key = funcData.GetId();
                if (key == "skill" || key == "skilldsl") {
                    Dsl.FunctionData callData = funcData.LowerOrderFunction;
                    if (null != callData) {
                        int dslId = id;
                        if (callData.HaveParam()) {
                            dslId = int.Parse(callData.GetParamId(0));
                        }
                        SkillInstance instance = new SkillInstance();
                        instance.Init(funcData);
                        instance.OuterDslSkillId = dslId;
                        if (!m_SkillInstances.ContainsKey(dslId)) {
                            m_SkillInstances.Add(dslId, instance);
                        } else {
                            m_SkillInstances[dslId] = instance;
                        }

                        LogSystem.Debug("ParseSkill {0}", dslId);
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
