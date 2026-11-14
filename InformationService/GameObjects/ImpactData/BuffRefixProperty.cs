using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    public static class BuffRefixProperty
    {
        public static void RefixCharacterProperty(EntityInfo entity, int buffId, float factor)
        {
            BuffConfig cfg = BuffConfigProvider.Instance.GetBuffConfig(buffId);
            if (null == cfg) return;
            for (int i = 0; i < cfg.Attrs.Count; ++i) {
                int attr = cfg.Attrs[i];
                // State flags are reference counters, not numeric attributes.
                if (attr >= (int)CharacterPropertyEnum.x3001_眩晕 && attr < (int)CharacterPropertyEnum.x4001_职业) continue;
                entity.ActualProperty.IncreaseInt((CharacterPropertyEnum)attr, (int)(cfg.Values[i] * factor));
            }
        }
    }
}
