using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    public sealed class BuffConfigProvider
    {
        public BuffConfig GetBuffConfig(int id)
        {
            BuffConfig cfg;
            if (m_Datas.TryGetValue(id, out cfg)) return cfg;
            TableConfig.BuffData row = TableConfig.BuffDataProvider.Instance.GetBuffData(id);
            if (null == row) return null;
            cfg = Build(row);
            m_Datas.Add(id, cfg);
            return cfg;
        }
        public void Clear() { m_Datas.Clear(); }

        private static BuffConfig Build(TableConfig.BuffData row)
        {
            BuffConfig cfg = new BuffConfig();
            cfg.BuffId = row.id;
            cfg.Desc = row.desc;
            int[] attrs = { row.attr1, row.attr_2, row.attr3, row.attr4, row.attr5, row.attr6, row.attr7, row.attr8 };
            int[] vals = { row.value1, row.value2, row.value3, row.value4, row.value5, row.value6, row.value7, row.value8 };
            for (int i = 0; i < attrs.Length; ++i) {
                if (attrs[i] > 0) { cfg.Attrs.Add(attrs[i]); cfg.Values.Add(vals[i]); }
            }
            return cfg;
        }

        private Dictionary<int, BuffConfig> m_Datas = new Dictionary<int, BuffConfig>();
        public static BuffConfigProvider Instance { get { return s_Instance; } }
        private static BuffConfigProvider s_Instance = new BuffConfigProvider();
    }
}
