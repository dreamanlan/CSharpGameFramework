using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    public static class ImpactTimeUtility
    {
        public static long NowMs() { return DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond; }
    }
}
