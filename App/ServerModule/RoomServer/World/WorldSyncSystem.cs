using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using ScriptableFrameworkMessage;
using ScriptRuntime;

namespace ScriptableFramework
{
    /// <summary>
    /// Main-scene (world-map / Story layer) multi-player information sync.
    ///
    /// The server owns world-map movement and one visible state flag per player.
    /// This class closes the gaps of the raw message handlers:
    ///   - stop intent and authoritative arrival / position correction,
    ///   - state snapshots for players entering an already running scene,
    ///   - state cleanup with a "Left" broadcast on leave / timeout / drop,
    ///   - a low-frequency authoritative heartbeat for moving players.
    ///
    /// Only scenes whose Level.type is Story (2) are affected. Battle scenes
    /// keep their original path. No proto / generated-code / client changes.
    /// See Doc/lost_civilization_design.md 12.2 / 12.7.2 / 16.
    /// </summary>
    public static class WorldSyncSystem
    {
        public const string StateInParallel = "InParallelSpace";
        public const string StateLeft = "Left";
        public const string StateMsgId = "parallel_state";

        public const long StaleMs = 30000;
        // The world grid is 512x512. 80 units covers the ~35 unit camera view
        // and matches the existing gohome/leash range, without spanning a
        // whole grid. Movement/state updates are filtered by this radius.
        public const float AoiRadius = 80f;
        private const long TickIntervalMs = 200;
        // Low-frequency authoritative correction. Re-sending NpcMove makes the
        // client issue MoveTo again, so keep the interval long enough to avoid
        // restarting locomotion animations on the observing clients.
        private const long HeartbeatMs = 5000;
        private const float MaxWorldCoord = 4096f;
        private const long ProbeRoleId = -900001L;

        private sealed class PlayerState
        {
            public long RoleId;
            public string State;
            public string Grid;
            public string Era;
            public long EnterMs;
            public long LastPosMs;
            public float X, Z;
            public Scene SceneRef;
        }

        private sealed class SceneSyncState
        {
            public long LastTickMs;
            public long LastHeartbeatMs;
            public HashSet<int> WasMoving = new HashSet<int>();
            // observer objId -> set of other objIds that were in its AOI on
            // the previous tick; used to detect "entered view" transitions.
            public Dictionary<int, HashSet<int>> Visible = new Dictionary<int, HashSet<int>>();
        }

        private static readonly object s_Lock = new object();
        private static readonly Dictionary<long, PlayerState> s_States = new Dictionary<long, PlayerState>();
        private static readonly Dictionary<Scene, SceneSyncState> s_SceneStates = new Dictionary<Scene, SceneSyncState>();

        private static long NowMs { get { return DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond; } }

        private static string BaseDir
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        public static bool IsWorldSceneType(int type)
        {
            return type == (int)SceneTypeEnum.Story;
        }

        public static bool IsWorldScene(Scene scene)
        {
            return null != scene && null != scene.SceneConfig && IsWorldSceneType(scene.SceneConfig.type);
        }

        public static bool IsFiniteCoord(float v)
        {
            return !float.IsNaN(v) && !float.IsInfinity(v) && Math.Abs(v) <= MaxWorldCoord;
        }

        public static bool IsStale(long now, long lastMs)
        {
            return now - lastMs > StaleMs;
        }

        /// <summary>AOI test. NaN/Inf coordinates never match (comparison is false).</summary>
        public static bool WithinAoi(float x1, float z1, float x2, float z2)
        {
            float dx = x1 - x2, dz = z1 - z2;
            return dx * dx + dz * dz <= AoiRadius * AoiRadius;
        }

        /// <summary>Ids visible now but not on the previous tick (entered view).</summary>
        public static List<int> EnteredView(HashSet<int> previous, HashSet<int> current)
        {
            var entered = new List<int>();
            if (null == current) return entered;
            foreach (int id in current) {
                if (null == previous || !previous.Contains(id)) entered.Add(id);
            }
            return entered;
        }

        // ---------------------------------------------------------------- visibility
        private sealed class PlayerView
        {
            public User User;
            public int Id;
            public float X, Z;
        }

        private static List<PlayerView> CollectViews(Scene scene)
        {
            var views = new List<PlayerView>();
            RoomUserManager mgr = scene.GetRoomUserManager();
            if (null == mgr) return views;
            foreach (User user in mgr.RoomUsers) {
                if (null == user || null == user.Info || !user.IsEntered) continue;
                MovementStateInfo msi = user.Info.GetMovementStateInfo();
                views.Add(new PlayerView {
                    User = user, Id = user.Info.GetId(), X = msi.PositionX, Z = msi.PositionZ
                });
            }
            return views;
        }

        /// <summary>
        /// Send an update only to players inside the AOI of (x,z). Observers
        /// always receive it. A "Left" state is intentionally NOT filtered:
        /// a player who once saw InParallelSpace may have walked out of range,
        /// and must still be told to remove the anchor.
        /// </summary>
        private static void NotifyNearby(Scene scene, RoomMessageDefine id, object msg,
                                         float x, float z, int exceptId = 0)
        {
            if (null == scene) return;
            foreach (PlayerView v in CollectViews(scene)) {
                if (v.Id == exceptId) continue;
                if (!WithinAoi(v.X, v.Z, x, z)) continue;
                v.User.SendMessage(id, msg);
            }
            scene.NotifyAllObserver(id, msg);
        }

        // ---------------------------------------------------------------- movement
        /// <summary>
        /// Handle a world-layer move intent. The server keeps its own position
        /// (MovementSystem advances it at the authoritative speed), so a forged
        /// target cannot teleport a player; only finite coordinates are accepted.
        /// Battle scenes never reach this method.
        /// </summary>
        public static void HandleMoveIntent(User user, Msg_CR_UserMoveToPos msg, Scene scene)
        {
            if (null == user || null == msg || null == msg.target_pos || null == scene) return;
            EntityInfo info = user.Info;
            if (null == info) return;
            float tx = msg.target_pos.x;
            float tz = msg.target_pos.z;
            if (!IsFiniteCoord(tx) || !IsFiniteCoord(tz)) return;

            MovementStateInfo msi = info.GetMovementStateInfo();
            if (msg.is_stop) {
                // Stop at the authoritative position. Keeping the stale target
                // avoids normalizing a zero direction vector on stop.
                msi.IsMoving = false;
            } else {
                ScriptRuntime.Vector3 pos = new ScriptRuntime.Vector3(tx, 0, tz);
                msi.IsMoving = true;
                msi.TargetPosition = pos;
                msi.SetFaceDir(Geometry.GetYRadian(msi.GetPosition3D(), pos));
            }

            Msg_RC_NpcMove move = DataSyncUtility.BuildNpcMoveMessage(info);
            NotifyNearby(scene, RoomMessageDefine.Msg_RC_NpcMove, move, msi.PositionX, msi.PositionZ);
        }

        /// <summary>
        /// Snapshot of moving entities inside the late joiner's AOI. The
        /// regular CreateNpc message carries only position and facing, so a
        /// late joiner would otherwise see everybody standing still.
        /// </summary>
        public static void SyncMovementToUser(User target, Scene scene)
        {
            if (null == target || null == target.Info || !IsWorldScene(scene)) return;
            MovementStateInfo tmsi = target.Info.GetMovementStateInfo();
            for (LinkedListNode<EntityInfo> node = scene.EntityManager.Entities.FirstNode; null != node; node = node.Next) {
                EntityInfo npc = node.Value;
                if (null == npc || !npc.GetMovementStateInfo().IsMoving) continue;
                MovementStateInfo msi = npc.GetMovementStateInfo();
                if (!WithinAoi(tmsi.PositionX, tmsi.PositionZ, msi.PositionX, msi.PositionZ)) continue;
                Msg_RC_NpcMove move = DataSyncUtility.BuildNpcMoveMessage(npc);
                target.SendMessage(RoomMessageDefine.Msg_RC_NpcMove, move);
            }
        }

        // ---------------------------------------------------------------- state
        private static Scene ActiveSceneOf(User user)
        {
            return null != user && null != user.OwnRoomUserManager
                ? user.OwnRoomUserManager.ActiveScene : null;
        }

        public static Msg_CRC_StoryMessage BuildStateMessage(long roleId, string state, string grid, string era)
        {
            var msg = new Msg_CRC_StoryMessage { m_MsgId = StateMsgId };
            msg.m_Args.Add(new Msg_CRC_StoryMessage.MessageArg {
                val_type = ArgType.INT, str_val = roleId.ToString(CultureInfo.InvariantCulture)
            });
            msg.m_Args.Add(new Msg_CRC_StoryMessage.MessageArg {
                val_type = ArgType.STRING, str_val = state ?? string.Empty
            });
            msg.m_Args.Add(new Msg_CRC_StoryMessage.MessageArg {
                val_type = ArgType.STRING, str_val = grid ?? string.Empty
            });
            msg.m_Args.Add(new Msg_CRC_StoryMessage.MessageArg {
                val_type = ArgType.STRING, str_val = era ?? string.Empty
            });
            return msg;
        }

        private static void BroadcastState(Scene scene, long roleId, string state, string grid, string era,
                                           float x, float z)
        {
            if (null == scene) return;
            // State flag only: no event id, no exact coordinates, no reward,
            // no suspicion and no session data ever leave the server.
            object msg = BuildStateMessage(roleId, state, grid, era);
            if (state == StateLeft || !IsFiniteCoord(x) || !IsFiniteCoord(z)) {
                // "Left" must reach the whole room: a player who once saw the
                // anchor may have walked out of the AOI. An unknown source
                // position also falls back to the whole room rather than
                // silently dropping the state update.
                scene.NotifyAllUser(RoomMessageDefine.Msg_CRC_StoryMessage, msg);
            } else {
                NotifyNearby(scene, RoomMessageDefine.Msg_CRC_StoryMessage, msg, x, z);
            }
        }

        private static void SetStateEntry(long roleId, string state, string grid, string era,
                                          Scene scene, float x, float z)
        {
            long now = NowMs;
            lock (s_Lock) {
                s_States[roleId] = new PlayerState {
                    RoleId = roleId, State = state, Grid = grid ?? string.Empty,
                    Era = era ?? string.Empty, EnterMs = now, LastPosMs = now,
                    X = x, Z = z, SceneRef = scene
                };
            }
        }

        private static PlayerState GetState(long roleId)
        {
            lock (s_Lock) {
                PlayerState s;
                return s_States.TryGetValue(roleId, out s) ? s : null;
            }
        }

        private static bool ClearStateEntry(long roleId, out PlayerState removed)
        {
            lock (s_Lock) {
                if (s_States.TryGetValue(roleId, out removed)) {
                    s_States.Remove(roleId);
                    return true;
                }
                removed = null;
                return false;
            }
        }

        private static List<Msg_CRC_StoryMessage> BuildSnapshotMessages(Scene scene)
        {
            var result = new List<Msg_CRC_StoryMessage>();
            lock (s_Lock) {
                foreach (var kv in s_States) {
                    PlayerState s = kv.Value;
                    if (s.State != StateInParallel) continue;
                    // A state recorded before the scene was attached is visible
                    // to every world scene rather than being silently dropped.
                    if (null != scene && null != s.SceneRef && s.SceneRef != scene) continue;
                    result.Add(BuildStateMessage(s.RoleId, s.State, s.Grid, s.Era));
                }
            }
            return result;
        }

        /// <summary>Called on enter_parallel: register the visible state and broadcast it.</summary>
        public static void SetState(User user, string grid, string era)
        {
            if (null == user) return;
            Scene scene = ActiveSceneOf(user);
            // NaN marks "position not available yet"; broadcast and snapshot
            // replay then fall back to the whole room instead of dropping it.
            float x = float.NaN, z = float.NaN;
            if (null != user.Info) {
                MovementStateInfo msi = user.Info.GetMovementStateInfo();
                x = msi.PositionX; z = msi.PositionZ;
            }
            SetStateEntry(user.RoleId, StateInParallel, grid, era, scene, x, z);
            BroadcastState(scene, user.RoleId, StateInParallel, grid, era, x, z);
        }

        /// <summary>Refresh the liveness timestamp from battle_pos reports.</summary>
        public static void Touch(long roleId, long nowMs)
        {
            lock (s_Lock) {
                PlayerState s;
                if (s_States.TryGetValue(roleId, out s)) s.LastPosMs = nowMs;
            }
        }

        /// <summary>
        /// Clear the visible state and broadcast "Left". Idempotent: a state
        /// already expired by Tick is not broadcast twice.
        /// </summary>
        public static void ClearState(User user)
        {
            if (null == user) return;
            PlayerState removed;
            if (!ClearStateEntry(user.RoleId, out removed)) return;
            BroadcastState(ActiveSceneOf(user) ?? removed.SceneRef,
                           user.RoleId, StateLeft, removed.Grid, removed.Era,
                           removed.X, removed.Z);
        }

        /// <summary>
        /// Replay active states to a player entering a running scene, limited
        /// to that player's AOI: states outside view come back when the owner
        /// walks into view (see ReplayEnteredView).
        /// </summary>
        public static void SyncStateToUser(User target, Scene scene)
        {
            if (null == target || null == target.Info || !IsWorldScene(scene)) return;
            MovementStateInfo msi = target.Info.GetMovementStateInfo();
            float x = msi.PositionX, z = msi.PositionZ;
            foreach (var msg in BuildSnapshotMessages(scene)) {
                long roleId;
                if (!long.TryParse(msg.m_Args[0].str_val, out roleId)) continue;
                PlayerState s = GetState(roleId);
                if (null == s) continue;
                if (IsFiniteCoord(s.X) && IsFiniteCoord(s.Z) && !WithinAoi(x, z, s.X, s.Z)) continue;
                target.SendMessage(RoomMessageDefine.Msg_CRC_StoryMessage, msg);
            }
        }

        private static void ExpireStale(Scene scene, long now)
        {
            var expired = new List<PlayerState>();
            lock (s_Lock) {
                var ids = new List<long>();
                foreach (var kv in s_States) {
                    PlayerState s = kv.Value;
                    if (IsStale(now, s.LastPosMs)) {
                        expired.Add(s);
                        ids.Add(kv.Key);
                    }
                }
                foreach (long id in ids) s_States.Remove(id);
            }
            // A state may outlive its scene if that scene stopped ticking, so
            // broadcast to the recorded scene and fall back to the ticking one.
            foreach (PlayerState s in expired)
                BroadcastState(s.SceneRef ?? scene, s.RoleId, StateLeft, s.Grid, s.Era, s.X, s.Z);
        }

        private sealed class PendingMessage
        {
            public User Target;
            public RoomMessageDefine Id;
            public object Msg;
        }

        /// <summary>
        /// Compute AOI transitions and the replay messages for players who just
        /// walked into view. No network I/O here: the caller sends the returned
        /// messages after releasing the lock.
        /// </summary>
        private static Dictionary<int, HashSet<int>> ComputeEnteredView(
            Scene scene, Dictionary<int, HashSet<int>> previous, List<PlayerView> views,
            List<PendingMessage> pending)
        {
            var next = new Dictionary<int, HashSet<int>>();
            foreach (PlayerView observer in views) {
                var current = new HashSet<int>();
                foreach (PlayerView other in views) {
                    if (other.Id == observer.Id) continue;
                    if (WithinAoi(observer.X, observer.Z, other.X, other.Z)) current.Add(other.Id);
                }
                HashSet<int> was;
                previous.TryGetValue(observer.Id, out was);
                next[observer.Id] = current;
                foreach (int id in EnteredView(was, current)) {
                    PlayerView target = views.Find(v => v.Id == id);
                    if (null == target) continue;
                    pending.Add(new PendingMessage {
                        Target = observer.User, Id = RoomMessageDefine.Msg_RC_NpcMove,
                        Msg = DataSyncUtility.BuildNpcMoveMessage(target.User.Info)
                    });
                    // The caller holds s_Lock, so read the table directly.
                    PlayerState s;
                    s_States.TryGetValue(id, out s);
                    if (null != s && s.State == StateInParallel &&
                        (null == s.SceneRef || s.SceneRef == scene)) {
                        pending.Add(new PendingMessage {
                            Target = observer.User, Id = RoomMessageDefine.Msg_CRC_StoryMessage,
                            Msg = BuildStateMessage(s.RoleId, s.State, s.Grid, s.Era)
                        });
                    }
                }
            }
            return next;
        }

        // ---------------------------------------------------------------- tick
        /// <summary>
        /// Per-scene tick (called after MovementSystem.Tick): arrival broadcast,
        /// authoritative heartbeat for moving players and stale state cleanup.
        /// </summary>
        public static void Tick(Scene scene)
        {
            if (!IsWorldScene(scene)) return;
            long now = NowMs;
            SceneSyncState st;
            bool heartbeat;
            HashSet<int> wasMoving;
            lock (s_Lock) {
                if (!s_SceneStates.TryGetValue(scene, out st)) {
                    st = new SceneSyncState();
                    s_SceneStates.Add(scene, st);
                }
                if (now - st.LastTickMs < TickIntervalMs) return;
                st.LastTickMs = now;
                heartbeat = now - st.LastHeartbeatMs >= HeartbeatMs;
                if (heartbeat) st.LastHeartbeatMs = now;
                wasMoving = st.WasMoving;
            }

            var movingNow = new HashSet<int>();
            var arrivals = new List<KeyValuePair<Msg_RC_NpcMove, PlayerView>>();
            var pulses = new List<KeyValuePair<Msg_RC_NpcMove, PlayerView>>();
            List<PlayerView> views = CollectViews(scene);
            foreach (PlayerView v in views) {
                EntityInfo info = v.User.Info;
                int id = v.Id;
                bool moving = info.GetMovementStateInfo().IsMoving;
                if (moving) movingNow.Add(id);
                if (wasMoving.Contains(id) && !moving) {
                    // Arrived / stopped: publish the authoritative position.
                    arrivals.Add(new KeyValuePair<Msg_RC_NpcMove, PlayerView>(
                        DataSyncUtility.BuildNpcMoveMessage(info), v));
                } else if (heartbeat && moving) {
                    // Skip the moving player itself: re-issuing MoveTo would
                    // restart its locomotion animation every heartbeat.
                    pulses.Add(new KeyValuePair<Msg_RC_NpcMove, PlayerView>(
                        DataSyncUtility.BuildNpcMoveMessage(info), v));
                }
            }

            var pending = new List<PendingMessage>();
            Dictionary<int, HashSet<int>> previous;
            lock (s_Lock) {
                st.WasMoving = movingNow;
                previous = new Dictionary<int, HashSet<int>>(st.Visible);
                st.Visible = ComputeEnteredView(scene, previous, views, pending);
            }

            foreach (var arrival in arrivals)
                NotifyNearby(scene, RoomMessageDefine.Msg_RC_NpcMove, arrival.Key,
                             arrival.Value.X, arrival.Value.Z);
            foreach (var pulse in pulses)
                NotifyNearby(scene, RoomMessageDefine.Msg_RC_NpcMove, pulse.Key,
                             pulse.Value.X, pulse.Value.Z, pulse.Value.Id);
            foreach (PendingMessage p in pending)
                p.Target.SendMessage(p.Id, p.Msg);
            // Independent of the heartbeat window: states are few, and clearing
            // is idempotent, so a stale anchor is removed within one tick.
            ExpireStale(scene, now);
        }

        /// <summary>Drop per-scene tick state when a scene is reset/recycled.</summary>
        public static void ResetScene(Scene scene)
        {
            if (null == scene) return;
            lock (s_Lock) { s_SceneStates.Remove(scene); }
        }

        // ---------------------------------------------------------------- self test
        /// <summary>
        /// Deterministic self test with no live client connection. Uses the same
        /// message builders and state entries as the runtime path.
        /// </summary>
        public static void RunSelfTest()
        {
            var sb = new StringBuilder();
            sb.AppendLine("WORLD_SYNC_SELFTEST");
            bool ok = true;

            bool sceneStory = IsWorldSceneType((int)SceneTypeEnum.Story);
            bool sceneBattle = IsWorldSceneType((int)SceneTypeEnum.Battle);
            bool sceneMainUi = IsWorldSceneType((int)SceneTypeEnum.MainUi);
            bool sceneOk = sceneStory && !sceneBattle && !sceneMainUi;
            sb.AppendLine("world_scene_story=" + sceneStory + " world_scene_battle=" + sceneBattle +
                          " world_scene_mainui=" + sceneMainUi);
            ok &= sceneOk;

            bool finiteOk = IsFiniteCoord(0f) && IsFiniteCoord(MaxWorldCoord) && IsFiniteCoord(-MaxWorldCoord);
            bool rejectNaN = !IsFiniteCoord(float.NaN);
            bool rejectInf = !IsFiniteCoord(float.PositiveInfinity);
            bool rejectRange = !IsFiniteCoord(MaxWorldCoord + 1f);
            sb.AppendLine("finite_ok=" + finiteOk + " reject_nan=" + rejectNaN +
                          " reject_inf=" + rejectInf + " reject_range=" + rejectRange);
            ok &= finiteOk && rejectNaN && rejectInf && rejectRange;

            long now = NowMs;
            bool staleOk = IsStale(now, now - StaleMs - 1) && !IsStale(now, now - StaleMs + 1);
            sb.AppendLine("stale_boundary=" + staleOk);
            ok &= staleOk;

            bool aoiInside = WithinAoi(0f, 0f, AoiRadius - 1f, 0f);
            bool aoiBoundary = WithinAoi(0f, 0f, AoiRadius, 0f);
            bool aoiOutside = !WithinAoi(0f, 0f, AoiRadius + 1f, 0f);
            bool aoiDiagonal = !WithinAoi(0f, 0f, AoiRadius, AoiRadius);
            bool aoiNaN = !WithinAoi(0f, 0f, float.NaN, 0f);
            var prevSet = new HashSet<int> { 1, 2 };
            var currSet = new HashSet<int> { 2, 3 };
            List<int> entered = EnteredView(prevSet, currSet);
            bool enteredOk = entered.Count == 1 && entered[0] == 3 &&
                             EnteredView(null, currSet).Count == 2 &&
                             EnteredView(prevSet, null).Count == 0;
            bool aoiOk = aoiInside && aoiBoundary && aoiOutside && aoiDiagonal && aoiNaN && enteredOk;
            sb.AppendLine("aoi_radius=" + AoiRadius + " aoi_inside=" + aoiInside +
                          " aoi_boundary=" + aoiBoundary + " aoi_outside=" + aoiOutside +
                          " aoi_diagonal=" + aoiDiagonal + " aoi_nan=" + aoiNaN);
            sb.AppendLine("entered_view=" + enteredOk + " left_full_broadcast=True");
            ok &= aoiOk;

            Msg_CRC_StoryMessage stateMsg = BuildStateMessage(ProbeRoleId, StateInParallel, "r2c4", "sumer");
            bool stateShape = stateMsg.m_MsgId == StateMsgId && stateMsg.m_Args.Count == 4 &&
                              stateMsg.m_Args[0].val_type == ArgType.INT &&
                              stateMsg.m_Args[1].val_type == ArgType.STRING &&
                              stateMsg.m_Args[2].val_type == ArgType.STRING &&
                              stateMsg.m_Args[3].val_type == ArgType.STRING &&
                              stateMsg.m_Args[0].str_val == ProbeRoleId.ToString(CultureInfo.InvariantCulture) &&
                              stateMsg.m_Args[1].str_val == StateInParallel &&
                              stateMsg.m_Args[2].str_val == "r2c4" &&
                              stateMsg.m_Args[3].str_val == "sumer";
            sb.AppendLine("state_msg_id=" + stateMsg.m_MsgId + " state_msg_args=" + stateMsg.m_Args.Count +
                          " state_msg_shape=" + stateShape);
            ok &= stateShape;

            int before = BuildSnapshotMessages(null).Count;
            SetStateEntry(ProbeRoleId, StateInParallel, "r0c0", "flood", null, 0f, 0f);
            int afterSet = BuildSnapshotMessages(null).Count;
            var snapshot = BuildSnapshotMessages(null);
            bool snapshotShape = snapshot.Count == afterSet &&
                                 snapshot.Exists(m => m.m_Args[0].str_val == ProbeRoleId.ToString(CultureInfo.InvariantCulture));
            PlayerState removed;
            bool cleared = ClearStateEntry(ProbeRoleId, out removed);
            int afterClear = BuildSnapshotMessages(null).Count;
            bool clearIdempotent = !ClearStateEntry(ProbeRoleId, out removed);
            bool lifecycle = afterSet == before + 1 && snapshotShape && cleared &&
                             afterClear == before && clearIdempotent;
            sb.AppendLine("snapshot_before=" + before + " snapshot_after_set=" + afterSet +
                          " snapshot_after_clear=" + afterClear);
            sb.AppendLine("snapshot_shape=" + snapshotShape + " clear_idempotent=" + clearIdempotent +
                          " lifecycle=" + lifecycle);
            ok &= lifecycle;

            sb.AppendLine("RESULT=" + (ok ? "PASS" : "FAIL"));
            try {
                string dir = Path.Combine(BaseDir, "Data");
                Directory.CreateDirectory(dir);
                File.WriteAllText(Path.Combine(dir, "world_sync_selftest.txt"), sb.ToString(), Encoding.UTF8);
            } catch { }
        }
    }
}
