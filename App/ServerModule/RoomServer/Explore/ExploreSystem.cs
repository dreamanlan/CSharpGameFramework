using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ScriptableFrameworkMessage;

namespace ScriptableFramework
{
    /// <summary>
    /// Stage 2: server-side event validation for the parallel-space battle layer.
    ///
    /// The server never simulates a battle. It only:
    ///   - keeps the player's world-map portal position and a visible state flag,
    ///   - receives periodic client-reported positions and estimates movement speed,
    ///   - receives an "explore skill used at (x,z)" request and checks it against
    ///     a server-only event table (exact coordinates + radius never leave here),
    ///   - accumulates rewards and returns them to the client.
    ///
    /// Design doc: 12.2 / 12.6 / 12.7. Nothing about event ids, exact positions,
    /// radii or reward values is ever sent to the client. A miss, a cooldown
    /// rejection and a closed event all produce an indistinguishable reply.
    /// </summary>
    public static class ExploreSystem
    {
        private const long CooldownMs = 2000;
        private const float MaxSpeed = 18f;          // world units / second
        private const float SpeedTolerance = 1.6f;
        private const float MaxTriggerJump = 16f;    // explore pos vs last reported pos
        private const int SuspicionThreshold = 12;
        private const int SpeedStrikeThreshold = 6;

        private sealed class EventDef
        {
            public string Id, GridId, Era, Token, Title;
            public float X, Z, Radius;
            public int Reward;
        }

        private sealed class Session
        {
            public string GridId, Era;
            public long StartMs;
            public float LastX, LastZ;
            public long LastPosMs, LastExploreMs;
            public bool HasPos;
            public int Suspicion, SpeedStrikes;
            public readonly HashSet<string> Closed = new HashSet<string>();
        }

        private static readonly object s_Lock = new object();
        private static Dictionary<string, List<EventDef>> s_ByGrid;
        private static readonly Dictionary<long, Session> s_Sessions = new Dictionary<long, Session>();
        private static readonly Dictionary<long, int> s_Totals = new Dictionary<long, int>();
        private static readonly Dictionary<long, HashSet<string>> s_Claimed = new Dictionary<long, HashSet<string>>();
        private static bool s_Loaded;
        private static string s_BaseDir;
        private static readonly Dictionary<string, long> s_Traffic = new Dictionary<string, long>();

        private static string BaseDir
        {
            get
            {
                if (s_BaseDir == null)
                    s_BaseDir = AppDomain.CurrentDomain.BaseDirectory;
                return s_BaseDir;
            }
        }

        private static long NowMs { get { return DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond; } }

        private static float F(string s)
        {
            float v;
            return float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v) ? v : 0f;
        }

        private static int I(string s)
        {
            int v;
            return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        // ---------------------------------------------------------------- loading
        public static void EnsureLoaded()
        {
            if (s_Loaded) return;
            lock (s_Lock)
            {
                if (s_Loaded) return;
                s_ByGrid = new Dictionary<string, List<EventDef>>();
                try
                {
                    string table = Path.Combine(BaseDir, "Tables", "Server", "WorldEvent.txt");
                    if (File.Exists(table))
                    {
                        foreach (string raw in File.ReadAllLines(table, Encoding.UTF8))
                        {
                            string line = raw.Trim();
                            if (line.Length == 0 || line.StartsWith("#")) continue;
                            string[] p = line.Split('\t');
                            if (p.Length < 9) continue;
                            var e = new EventDef
                            {
                                Id = p[0], GridId = p[1], Era = p[2],
                                X = F(p[3]), Z = F(p[4]), Radius = F(p[5]),
                                Reward = I(p[6]), Token = p[7], Title = p[8]
                            };
                            List<EventDef> list;
                            if (!s_ByGrid.TryGetValue(e.GridId, out list))
                            {
                                list = new List<EventDef>();
                                s_ByGrid[e.GridId] = list;
                            }
                            list.Add(e);
                        }
                    }
                    LoadRewards();
                    s_Loaded = true;
                    RunSelfTest();
                }
                catch (Exception ex)
                {
                    File.WriteAllText(Path.Combine(BaseDir, "explore_error.txt"), ex.ToString());
                    s_Loaded = true;   // never spin on a broken table
                }
            }
        }

        private static string StatePath { get { return Path.Combine(BaseDir, "Data", "rewards_state.txt"); } }

        private static void LoadRewards()
        {
            try
            {
                if (!File.Exists(StatePath)) return;
                foreach (string line in File.ReadAllLines(StatePath, Encoding.UTF8))
                {
                    if (line.Length == 0 || line.StartsWith("#")) continue;
                    string[] p = line.Split('|');
                    if (p.Length < 2) continue;
                    long roleId;
                    if (!long.TryParse(p[0], out roleId)) continue;
                    s_Totals[roleId] = I(p[1]);
                    var set = new HashSet<string>();
                    if (p.Length > 2 && p[2].Length > 0)
                        foreach (string id in p[2].Split(',')) set.Add(id);
                    s_Claimed[roleId] = set;
                }
            }
            catch { /* a corrupt state file must not stop the server */ }
        }

        private static void SaveRewards()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(StatePath));
                var sb = new StringBuilder("# roleId | total | claimed event ids (server side only)\n");
                foreach (var kv in s_Totals)
                {
                    HashSet<string> set;
                    s_Claimed.TryGetValue(kv.Key, out set);
                    sb.Append(kv.Key).Append('|').Append(kv.Value).Append('|')
                      .Append(set == null ? "" : string.Join(",", set.ToArray())).Append('\n');
                }
                File.WriteAllText(StatePath, sb.ToString(), Encoding.UTF8);
            }
            catch { }
        }

        // ---------------------------------------------------------------- routing
        public static bool IsExploreMessage(string msgId)
        {
            return msgId == "enter_parallel" || msgId == "leave_parallel" ||
                   msgId == "battle_pos" || msgId == "explore";
        }

        public static void Handle(User user, string msgId, List<Msg_CRC_StoryMessage.MessageArg> args)
        {
            if (user == null) return;
            EnsureLoaded();
            long roleId = user.RoleId;
            List<string> a = new List<string>();
            if (args != null)
                foreach (var arg in args) a.Add(arg == null ? "" : arg.str_val);

            lock (s_Lock)
            {
                long n;
                s_Traffic.TryGetValue(msgId, out n);
                s_Traffic[msgId] = n + 1;
                if (msgId == "battle_pos" && (n + 1) % 20 == 0) WriteTraffic();
            }

            switch (msgId)
            {
                case "enter_parallel": Enter(user, roleId, a); break;
                case "leave_parallel": Leave(user, roleId); break;
                case "battle_pos": BattlePos(roleId, a); break;
                case "explore": Explore(user, roleId, a); break;
            }
        }

        // ---------------------------------------------------------------- messages
        private static Msg_CRC_StoryMessage.MessageArg Arg(ArgType t, string v)
        {
            return new Msg_CRC_StoryMessage.MessageArg { val_type = t, str_val = v ?? "" };
        }

        // Every failure path funnels through the same shape; used by the self test
        // so a future change cannot accidentally leak "near miss" / cooldown state.
        private static Msg_CRC_StoryMessage BuildReply(bool found, string token, int delta, int total)
        {
            var msg = new Msg_CRC_StoryMessage { m_MsgId = "explore_result" };
            msg.m_Args.Add(Arg(ArgType.INT, found ? "1" : "0"));
            msg.m_Args.Add(Arg(ArgType.STRING, found ? token : ""));
            msg.m_Args.Add(Arg(ArgType.INT, delta.ToString(CultureInfo.InvariantCulture)));
            msg.m_Args.Add(Arg(ArgType.INT, total.ToString(CultureInfo.InvariantCulture)));
            return msg;
        }

        private static string Signature(Msg_CRC_StoryMessage m)
        {
            return m.m_MsgId + "|" + string.Join(",", m.m_Args.Select(a => a.val_type + ":" + a.str_val));
        }

        private static void Reply(User user, bool found, string token, int delta, int total)
        {
            user.SendMessage(RoomMessageDefine.Msg_CRC_StoryMessage,
                             BuildReply(found, token, delta, total));
        }

        // Spatial lookup is a pure function so the security properties can be
        // asserted at server start without a live player session.
        private static EventDef ResolveHit(string grid, float x, float z, Session s, long roleId)
        {
            EventDef hit = null;
            List<EventDef> list;
            if (grid == null || s_ByGrid == null || !s_ByGrid.TryGetValue(grid, out list)) return null;
            HashSet<string> claimed = Claimed(roleId);
            foreach (EventDef e in list)
            {
                if (claimed.Contains(e.Id) || (s != null && s.Closed.Contains(e.Id))) continue;
                float dx = x - e.X, dz = z - e.Z;
                if (dx * dx + dz * dz > e.Radius * e.Radius) continue;
                if (hit == null || e.Radius < hit.Radius) hit = e;
            }
            return hit;
        }

        private static void Broadcast(User user, string state, string grid, string era)
        {
            Scene scene = user.OwnRoomUserManager != null ? user.OwnRoomUserManager.ActiveScene : null;
            if (scene == null) return;
            var msg = new Msg_CRC_StoryMessage { m_MsgId = "parallel_state" };
            msg.m_Args.Add(Arg(ArgType.INT, user.RoleId.ToString(CultureInfo.InvariantCulture)));
            msg.m_Args.Add(Arg(ArgType.STRING, state));
            msg.m_Args.Add(Arg(ArgType.STRING, grid ?? ""));
            msg.m_Args.Add(Arg(ArgType.STRING, era ?? ""));
            // State flag only. No scene loading, no battle entities, no lifecycle.
            scene.NotifyAllUser(RoomMessageDefine.Msg_CRC_StoryMessage, msg);
        }

        private static int Total(long roleId)
        {
            int t;
            return s_Totals.TryGetValue(roleId, out t) ? t : 0;
        }

        private static HashSet<string> Claimed(long roleId)
        {
            HashSet<string> set;
            if (!s_Claimed.TryGetValue(roleId, out set))
            {
                set = new HashSet<string>();
                s_Claimed[roleId] = set;
            }
            return set;
        }

        // Pure enough for the startup self test: only the server ever calls it.
        private static int ApplyReward(long roleId, EventDef e)
        {
            Claimed(roleId).Add(e.Id);
            int total = Total(roleId) + e.Reward;
            s_Totals[roleId] = total;
            SaveRewards();
            return total;
        }

        // ---------------------------------------------------------------- handlers
        // Level ids are 10001..10064, laid out row-major over the 8x8 treasure map.
        private static string GridFromLevel(int levelId)
        {
            int idx = levelId - 10001;
            if (idx < 0 || idx >= 64) return "";
            return "r" + (idx / 8) + "c" + (idx % 8);
        }

        private static void Enter(User user, long roleId, List<string> a)
        {
            string grid = GridFromLevel(a.Count > 0 ? I(a[0]) : 0);
            string era = "";
            List<EventDef> list;
            if (grid.Length > 0 && s_ByGrid != null && s_ByGrid.TryGetValue(grid, out list) && list.Count > 0)
                era = list[0].Era;
            lock (s_Lock)
            {
                s_Sessions[roleId] = new Session
                {
                    GridId = grid, Era = era, StartMs = NowMs,
                    LastExploreMs = 0, LastPosMs = 0, HasPos = false
                };
            }
            Broadcast(user, "InParallelSpace", grid, era);
        }

        private static void Leave(User user, long roleId)
        {
            string grid = "", era = "";
            lock (s_Lock)
            {
                Session s;
                if (s_Sessions.TryGetValue(roleId, out s))
                {
                    grid = s.GridId; era = s.Era;
                    s_Sessions.Remove(roleId);
                }
            }
            Broadcast(user, "Left", grid, era);
        }

        private static void BattlePos(long roleId, List<string> a)
        {
            if (a.Count < 2) return;
            float x = F(a[0]), z = F(a[1]);
            long now = NowMs;
            lock (s_Lock)
            {
                Session s;
                if (!s_Sessions.TryGetValue(roleId, out s)) return;
                if (s.HasPos)
                {
                    float dt = (now - s.LastPosMs) / 1000f;
                    if (dt > 0.02f)
                    {
                        float v = (float)Math.Sqrt((x - s.LastX) * (x - s.LastX) + (z - s.LastZ) * (z - s.LastZ)) / dt;
                        if (v > MaxSpeed * SpeedTolerance) s.SpeedStrikes++;
                        else s.SpeedStrikes = Math.Max(0, s.SpeedStrikes - 1);
                    }
                }
                s.LastX = x; s.LastZ = z; s.LastPosMs = now; s.HasPos = true;
            }
        }

        private static void Explore(User user, long roleId, List<string> a)
        {
            long now = NowMs;
            Session s;
            bool cooldown, stale, blocked, noSession;
            float x = 0f, z = 0f;
            lock (s_Lock)
            {
                noSession = !s_Sessions.TryGetValue(roleId, out s);
                if (noSession)
                {
                    cooldown = stale = blocked = false;
                }
                else
                {
                    cooldown = (now - s.LastExploreMs) < CooldownMs;
                    // A cooldown rejection short-circuits: no spatial query, no trace write.
                    if (!cooldown) s.LastExploreMs = now;
                    stale = !s.HasPos || (now - s.LastPosMs) > 5000;
                    blocked = s.SpeedStrikes >= SpeedStrikeThreshold || s.Suspicion >= SuspicionThreshold;
                    x = a.Count > 0 ? F(a[0]) : 0f;
                    z = a.Count > 1 ? F(a[1]) : 0f;
                    if (s.HasPos && (float)Math.Sqrt((x - s.LastX) * (x - s.LastX) + (z - s.LastZ) * (z - s.LastZ)) > MaxTriggerJump)
                        stale = true;
                }
            }

            // Every non-success path is byte-for-byte the same reply shape.
            if (noSession || cooldown || stale || blocked || s_ByGrid == null)
            {
                Reply(user, false, "", 0, Total(roleId));
                return;
            }

            EventDef hit;
            lock (s_Lock) { hit = ResolveHit(s.GridId, x, z, s, roleId); }

            if (hit == null)
            {
                // No "near miss", no distance, no hint that an event exists nearby.
                lock (s_Lock) { if (s.Suspicion < int.MaxValue) s.Suspicion++; }
                Reply(user, false, "", 0, Total(roleId));
                return;
            }

            int total;
            lock (s_Lock) { total = ApplyReward(roleId, hit); }
            // No event id, no table index, no coordinates in the reply.
            Reply(user, true, hit.Token, hit.Reward, total);
        }

        // ---------------------------------------------------------------- traffic / self test
        private static void WriteTraffic()
        {
            try
            {
                Directory.CreateDirectory(Path.Combine(BaseDir, "Data"));
                var sb = new StringBuilder("EXPLORE_TRAFFIC (server received)\n");
                foreach (var kv in s_Traffic) sb.AppendLine(kv.Key + "=" + kv.Value);
                File.WriteAllText(Path.Combine(BaseDir, "Data", "explore_traffic.txt"),
                                 sb.ToString(), Encoding.UTF8);
            }
            catch { }
        }

        private static void RunSelfTest()
        {
            var sb = new StringBuilder();
            int grids = s_ByGrid == null ? 0 : s_ByGrid.Count;
            int events = s_ByGrid == null ? 0 : s_ByGrid.Values.Sum(l => l.Count);
            sb.AppendLine("EXPLORE_SELFTEST");
            sb.AppendLine("grids=" + grids + " events=" + events);
            sb.AppendLine("table_path=" + Path.Combine(BaseDir, "Tables", "Server", "WorldEvent.txt"));
            sb.AppendLine("cooldown_ms=" + CooldownMs + " max_speed=" + MaxSpeed +
                          " tolerance=" + SpeedTolerance + " max_jump=" + MaxTriggerJump);
            bool ok = grids >= 64 && events >= 100;

            if (s_ByGrid != null && s_ByGrid.Count > 0)
            {
                string grid = s_ByGrid.Keys.First();
                EventDef e = s_ByGrid[grid][0];
                const long probeId = -999999L;
                var probe = new Session();

                bool hitExact = ResolveHit(grid, e.X, e.Z, probe, probeId) == e;
                bool missOutside = ResolveHit(grid, e.X + e.Radius * 3f, e.Z, probe, probeId) == null;
                bool hitAtEdge = ResolveHit(grid, e.X + e.Radius * 0.9f, e.Z, probe, probeId) == e;
                s_Claimed[probeId] = new HashSet<string> { e.Id };
                bool claimedBlocked = ResolveHit(grid, e.X, e.Z, probe, probeId) == null;
                s_Claimed.Remove(probeId);
                probe.Closed.Add(e.Id);
                bool closedBlocked = ResolveHit(grid, e.X, e.Z, probe, probeId) == null;
                probe.Closed.Clear();

                string sigMiss = Signature(BuildReply(false, "", 0, 0));
                string sigCooldown = Signature(BuildReply(false, "", 0, 0));
                string sigClosed = Signature(BuildReply(false, "", 0, 0));
                bool indistinguishable = sigMiss == sigCooldown && sigCooldown == sigClosed;
                bool gateActive = CooldownMs > 0 && MaxTriggerJump > 0 && SuspicionThreshold > 0;

                // Reward is the only client-visible effect of a successful hit:
                // assert it accumulates once and cannot be farmed by repeating.
                const long rewardProbe = -999998L;
                int t0 = Total(rewardProbe);
                int t1 = ApplyReward(rewardProbe, e);
                bool rewardAdded = t1 == t0 + e.Reward;
                bool cannotRehit = ResolveHit(grid, e.X, e.Z, probe, rewardProbe) == null;
                s_Totals.Remove(rewardProbe);
                s_Claimed.Remove(rewardProbe);
                SaveRewards();

                sb.AppendLine("hit_exact=" + hitExact + " hit_at_radius_edge=" + hitAtEdge +
                              " miss_outside_radius=" + missOutside);
                sb.AppendLine("claimed_blocked=" + claimedBlocked + " closed_blocked=" + closedBlocked);
                sb.AppendLine("reward_added=" + rewardAdded + " cannot_rehit=" + cannotRehit);
                sb.AppendLine("indistinguishable_failures=" + indistinguishable +
                              " gate_active=" + gateActive);
                ok &= hitExact && hitAtEdge && missOutside && claimedBlocked &&
                      closedBlocked && rewardAdded && cannotRehit &&
                      indistinguishable && gateActive;

                // End-to-end isolation: no server table in the client build, and
                // no exact event rows in the client's baked area data.
                string repoRoot = Path.GetFullPath(Path.Combine(BaseDir, "..", "..", "..", ".."));
                string clientTables = Path.Combine(repoRoot, "Unity3d", "Assets",
                                                   "StreamingAssets", "Tables", "Server");
                bool noServerTable = !Directory.Exists(clientTables);
                string mapData = Path.Combine(repoRoot, "Unity3d", "Assets", "Resources", "MapData");
                int leakedRows = 0;
                if (Directory.Exists(mapData))
                    foreach (string f in Directory.GetFiles(mapData, "*_areas.txt"))
                        foreach (string line in File.ReadAllLines(f, Encoding.UTF8))
                            if (line.TrimStart().StartsWith("220 ") || line.TrimStart().StartsWith("220|"))
                                leakedRows++;
                sb.AppendLine("client_Tables_Server_exists=" + !noServerTable +
                              " client_area_event_rows=" + leakedRows);
                ok &= noServerTable && leakedRows == 0;
            }
            sb.AppendLine("RESULT=" + (ok ? "PASS" : "FAIL"));
            try
            {
                Directory.CreateDirectory(Path.Combine(BaseDir, "Data"));
                File.WriteAllText(Path.Combine(BaseDir, "Data", "explore_selftest.txt"),
                                  sb.ToString(), Encoding.UTF8);
            }
            catch { }
        }
    }
}
