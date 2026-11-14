using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    internal static class ImpactInfoExtensions
    {
        public static ImpactLogicData Cfg(this ImpactInfo info)
        {
            return null == info ? null : ImpactLogicDataProvider.Instance.GetImpactLogicData(info.ImpactId);
        }
    }
}
