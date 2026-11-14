using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    // Per-attacker hit sequence used by logic 12/13/14/17.
    public static class ImpactHitCounter
    {
        private sealed class HitStat
        {
            public int Count;
            public int LastHitId;
            public long LastHitTime;
            public int KillHitId = -1;
            public int KillAddDamage;
            public int KillAddHits;
            public int RegisteredAdd;
            public int RegisteredHits;
        }
        public static void OnHit(EntityInfo sender, int damage, bool isKiller)
        {
            if (null == sender) return;
            HitStat stat = GetOrNew(sender.GetId());
            ++stat.Count;
            stat.LastHitId = stat.Count;
            stat.LastHitTime = ImpactTimeUtility.NowMs();
            if (isKiller && stat.RegisteredAdd > 0) {
                stat.KillHitId = stat.Count;
                stat.KillAddDamage = stat.RegisteredAdd;
                stat.KillAddHits = stat.RegisteredHits;
            }
        }
        public static int CountOf(int senderId) { return GetOrNew(senderId).Count; }
        public static int LastHitIdOf(int senderId) { return GetOrNew(senderId).LastHitId; }
        public static long LastHitTimeOf(int senderId) { return GetOrNew(senderId).LastHitTime; }
        public static void SetKillBonus(int senderId, int add, int hits)
        {
            HitStat stat = GetOrNew(senderId);
            stat.RegisteredAdd = add;
            stat.RegisteredHits = hits;
        }
        public static int TakeKillAddDamage(int senderId, int hitId)
        {
            HitStat stat = GetOrNew(senderId);
            if (stat.KillHitId < 0 || stat.KillAddDamage <= 0) return 0;
            if (hitId <= stat.KillHitId || hitId > stat.KillHitId + stat.KillAddHits) {
                stat.KillHitId = -1;
                stat.KillAddDamage = 0;
                return 0;
            }
            return stat.KillAddDamage;
        }
        public static void ClearKillBonus(int senderId)
        {
            HitStat stat = GetOrNew(senderId);
            stat.KillHitId = -1;
            stat.KillAddDamage = 0;
            stat.RegisteredAdd = 0;
        }
        private static HitStat GetOrNew(int id)
        {
            HitStat stat;
            if (!s_Stats.TryGetValue(id, out stat)) { stat = new HitStat(); s_Stats.Add(id, stat); }
            return stat;
        }
        private static Dictionary<int, HitStat> s_Stats = new Dictionary<int, HitStat>();
    }
}
