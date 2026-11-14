using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // One row of ImpactData.txt: logic id selects the behaviour, buffDataId selects pure attribute deltas.
    public sealed class ImpactLogicData
    {
        public int ImpactId;
        public int LogicId;
        public int GfxLogicId;
        public int ImpactType = (int)ScriptableFramework.ImpactType.BUFF;
        public int ImpactTime;
        public int StateId;
        public bool BreakSuperArmor;
        public bool IsDebuff;
        public bool IsIgnorePassiveCheck;
        public int DamageType;
        public int ElementType;
        public int DamageValue;
        public float DamageRate = 1.0f;
        public float LevelRate;
        public int BuffDataId;
        public int ParamNum;
        public float MaxDist;
        public List<string> ExtraParams = new List<string>();
        public List<string> EffectList = new List<string>();
        public List<int> Multiples = new List<int>();
        public List<int> Damages = new List<int>();

        public float CalcMaxMoveDistance() { return MaxDist; }
        public string GetParam(int ix, string def)
        {
            if (ix >= 0 && ix < ExtraParams.Count) return ExtraParams[ix];
            return def;
        }
        public float GetFloatParam(int ix, float def)
        {
            float val;
            if (ix >= 0 && ix < ExtraParams.Count && float.TryParse(ExtraParams[ix], out val)) return val;
            return def;
        }
        public int GetIntParam(int ix, int def)
        {
            int val;
            if (ix >= 0 && ix < ExtraParams.Count && int.TryParse(ExtraParams[ix], out val)) return val;
            return def;
        }
    }

    public sealed class ImpactLogicDataProvider
    {
        public ImpactLogicData GetImpactLogicData(int id)
        {
            ImpactLogicData data;
            if (m_Datas.TryGetValue(id, out data)) return data;
            TableConfig.ImpactData row = TableConfig.ImpactDataProvider.Instance.GetImpactData(id);
            if (null == row) return null;
            data = Build(row);
            m_Datas.Add(id, data);
            return data;
        }
        public void Clear() { m_Datas.Clear(); }

        private static ImpactLogicData Build(TableConfig.ImpactData row)
        {
            ImpactLogicData data = new ImpactLogicData();
            data.ImpactId = row.id;
            data.LogicId = row.logicId;
            data.GfxLogicId = row.gfxLogicId;
            data.ImpactType = row.impactType > 0 ? row.impactType : (int)ScriptableFramework.ImpactType.BUFF;
            data.ImpactTime = row.impactTime > 0 ? row.impactTime : row.duration;
            data.StateId = row.stateId;
            data.BreakSuperArmor = row.breakSuperArmor != 0;
            data.IsDebuff = row.isDebuff != 0;
            data.IsIgnorePassiveCheck = row.isIgnorePassiveCheck != 0;
            data.DamageType = row.damageType;
            data.ElementType = row.elementType;
            data.DamageValue = row.damageValue;
            data.DamageRate = row.damageRate;
            data.LevelRate = row.levelRate;
            data.BuffDataId = row.buffDataId;
            data.ParamNum = row.paramNum;
            data.MaxDist = row.maxDist;
            string[] ps = { row.param0, row.param1, row.param2, row.param3, row.param4, row.param5, row.param6, row.param7 };
            int ct = Math.Min(Math.Max(data.ParamNum, 0), ps.Length);
            for (int i = 0; i < ct; ++i) data.ExtraParams.Add(ps[i] ?? string.Empty);
            string[] es = { row.effect0, row.effect1, row.effect2, row.effect3, row.effect4 };
            for (int i = 0; i < es.Length; ++i) {
                if (!string.IsNullOrEmpty(es[i])) data.EffectList.Add(es[i]);
            }
            if (null != row.multiple) data.Multiples.AddRange(row.multiple);
            if (null != row.damage) data.Damages.AddRange(row.damage);
            return data;
        }

        private Dictionary<int, ImpactLogicData> m_Datas = new Dictionary<int, ImpactLogicData>();
        public static ImpactLogicDataProvider Instance { get { return s_Instance; } }
        private static ImpactLogicDataProvider s_Instance = new ImpactLogicDataProvider();
    }
}
