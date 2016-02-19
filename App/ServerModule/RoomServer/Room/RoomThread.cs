using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Messenger;
using GameFrameworkMessage;

namespace GameFramework
{
    /// <remarks>
    /// 注意这个类的internal方法，都应考虑跨线程调用是否安全！！！
    /// </remarks>
    internal class RoomThread : MyServerThread
    {
        // constructors------------------------------------------------------------
        internal RoomThread(RoomManager roomMgr)
        {
            room_mgr_ = roomMgr;
            cur_thread_id_ = thread_id_creator_++;
            tick_interval_ = 100;
            max_room_count_ = 0;
            active_room_ = new List<Room>();
            unused_room_ = new List<Room>();
            room_pool_ = new RoomPool();
            scene_pool_ = new ScenePool();
        }

        internal bool Init(uint tick_interval, uint room_count,
            UserPool userpool, Connector conn)
        {
            tick_interval_ = tick_interval;
            max_room_count_ = room_count;
            room_pool_.Init(max_room_count_);
            user_pool_ = userpool;
            connector_ = conn;
            LogSys.Log(LOG_TYPE.DEBUG, "thread {0} init ok.", cur_thread_id_);
            return true;
        }

        internal int IdleRoomCount()
        {
            return (int)(max_room_count_ - active_room_.Count);
        }

        internal void ActiveRoom(int roomid, int scenetype)
        {
            LogSys.Log(LOG_TYPE.INFO, "[0] active field room {0} scene {1} thread {2}", roomid, scenetype, cur_thread_id_);
            Room rm = room_pool_.NewRoom();
            if (null == rm) {
                LogSys.Log(LOG_TYPE.ERROR, "Failed active field room {0} in thread {1}",
                    roomid, cur_thread_id_);
                return;
            }
            LogSys.Log(LOG_TYPE.INFO, "[1] active field room {0} scene {1} thread {2}", roomid, scenetype, cur_thread_id_);
            rm.ScenePool = scene_pool_;
            rm.Init(roomid, scenetype, user_pool_, connector_);
            LogSys.Log(LOG_TYPE.INFO, "[2] active field room {0} scene {1} thread {2}", roomid, scenetype, cur_thread_id_);

            rm.MaxScore = 1000000;
            rm.MaxLevel = 999;
            rm.Monsters.Clear();
            rm.Hps.Clear();

            LogSys.Log(LOG_TYPE.INFO, "[3] active field room {0} scene {1} thread {2}", roomid, scenetype, cur_thread_id_);

            //工作全部完成后再加到激活房间列表，开始tick
            active_room_.Add(rm);

            LogSys.Log(LOG_TYPE.DEBUG, "active field room {0} in thread {1}",
                roomid, cur_thread_id_);
        }

        internal void AddUser(ulong guid, int roomId, User user)
        {
            Room room = GetRoomByID(roomId);
            if (null != room) {
                room.AddNewUser(user);
            }
        }

        internal void ChangeRoomScene(int roomid, int scenetype)
        {
            Room room = GetRoomByID(roomid);
            if (null != room) {
                room.ChangeScene(scenetype);
            }
        }

        internal void HandleEnterScene(ulong guid, int roomId, User user, PBChannel channel, int handle, uint seq)
        {
            Room room = GetRoomByID(roomId);
            if (null != room) {
                if (room.AddNewUser(user)) {
                    Msg_RL_EnterSceneResult replyBuilder = new Msg_RL_EnterSceneResult();
                    replyBuilder.UserGuid = guid;
                    replyBuilder.RoomID = roomId;
                    replyBuilder.Result = (int)SceneOperationResultEnum.Success;
                    channel.Send(replyBuilder);
                } else {
                    Msg_RL_EnterSceneResult replyBuilder = new Msg_RL_EnterSceneResult();
                    replyBuilder.UserGuid = guid;
                    replyBuilder.RoomID = roomId;
                    replyBuilder.Result = (int)SceneOperationResultEnum.User_Key_Exist;
                    channel.Send(replyBuilder);
                }
            } else {
                Msg_RL_EnterSceneResult replyBuilder = new Msg_RL_EnterSceneResult();
                replyBuilder.UserGuid = guid;
                replyBuilder.RoomID = roomId;
                replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                channel.Send(replyBuilder);
            }
        }

        internal void HandleChangeScene(ulong userGuid, int roomId, int targetRoomId, PBChannel channel, int handle, uint seq)
        {
            Room room = GetRoomByID(roomId);
            if (null != room) {
                User user = room.GetUserByGuid(userGuid);
                if (null != user) {
                    RemoveUserForChangeScene(room, user, userGuid, roomId, targetRoomId, channel, handle, seq);
                } else {
                    Msg_RL_ChangeSceneResult replyBuilder = new Msg_RL_ChangeSceneResult();
                    replyBuilder.UserGuid = userGuid;
                    replyBuilder.RoomID = roomId;
                    replyBuilder.TargetRoomID = targetRoomId;
                    replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_User;
                    channel.Send(replyBuilder);
                }
            } else {
                Msg_RL_ChangeSceneResult replyBuilder = new Msg_RL_ChangeSceneResult();
                replyBuilder.UserGuid = userGuid;
                replyBuilder.RoomID = roomId;
                replyBuilder.TargetRoomID = targetRoomId;
                replyBuilder.Result = (int)SceneOperationResultEnum.Cant_Find_Room;
                channel.Send(replyBuilder);
            }
        }

        internal void HandleReconnectUser(Msg_LR_ReconnectUser urMsg, PBChannel channel, int handle, uint seq)
        {
            Msg_RL_ReplyReconnectUser.ReconnectResultEnum result;
            User us = GetUserByGuid(urMsg.UserGuid);
            if (null != us) {
                if ((int)UserControlState.UserDropped == us.UserControlState) {
                    result = Msg_RL_ReplyReconnectUser.ReconnectResultEnum.Drop;
                } else {
                    result = Msg_RL_ReplyReconnectUser.ReconnectResultEnum.Online;
                }
            } else {
                result = Msg_RL_ReplyReconnectUser.ReconnectResultEnum.NotExist;
            }
            Msg_RL_ReplyReconnectUser replyBuilder = new Msg_RL_ReplyReconnectUser();
            replyBuilder.UserGuid = urMsg.UserGuid;
            replyBuilder.RoomID = urMsg.RoomID;
            replyBuilder.Result = (int)result;
            channel.Send(replyBuilder);
        }

        internal void HandleUserRelive(Msg_LR_UserReLive msg)
        {
            Room room = GetRoomByID(msg.RoomID);
            if (null != room) {
                Scene curScene = room.GetActiveScene();
                if (null != curScene) {
                    //curScene.DelayActionProcessor.QueueAction(curScene.OnUserRevive, msg);
                }
            }
        }

        internal void HandleUserQuit(Msg_LR_UserQuit msg, PBChannel channel)
        {
            Room room = GetRoomByID(msg.RoomID);
            if (null != room) {
                User user = room.GetUserByGuid(msg.UserGuid);
                if (null != user) {
                    room.DeleteUser(user);
                }
            }
            Msg_RL_UserQuit replyBuilder = new Msg_RL_UserQuit();
            replyBuilder.UserGuid = msg.UserGuid;
            replyBuilder.RoomID = msg.RoomID;
            channel.Send(replyBuilder);
        }

        internal void HandleReclaimItem(Msg_LR_ReclaimItem msg, PBChannel channel)
        {
            Room room = GetRoomByID(msg.RoomID);
            if (null != room) {
                User user = room.GetUserByGuid(msg.UserGuid);
                Scene curScene = room.GetActiveScene();
                if (null != curScene) {
                }
            }
        }

        internal void HandleRoomStoryMessage(Msg_LRL_StoryMessage msg, PBChannel channel)
        {
            Room room = GetRoomByID(msg.RoomId);
            if (null != room) {
                User user = room.GetUserByGuid(msg.UserGuid);
                Scene curScene = room.GetActiveScene();
                if (null != user && null != curScene) {
                    try {
                        string msgId = string.Format("server:{0}", msg.MsgId);
                        ArrayList args = new ArrayList();
                        args.Add(user.RoleId);
                        for (int i = 0; i < msg.Args.Count; i++) {
                            switch (msg.Args[i].val_type) {
                                case Msg_LRL_StoryMessage.ArgType.NULL://null
                                    args.Add(null);
                                    break;
                                case Msg_LRL_StoryMessage.ArgType.INT://int
                                    args.Add(int.Parse(msg.Args[i].str_val));
                                    break;
                                case Msg_LRL_StoryMessage.ArgType.FLOAT://float
                                    args.Add(float.Parse(msg.Args[i].str_val));
                                    break;
                                default://string
                                    args.Add(msg.Args[i].str_val);
                                    break;
                            }
                        }
                        object[] objArgs = args.ToArray();
                        curScene.StorySystem.SendMessage(msgId, objArgs);
                    } catch (Exception ex) {
                        LogSys.Log(LOG_TYPE.ERROR, "Msg_CRC_StoryMessage throw exception:{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                }
            }
        }

        private void RemoveUserForChangeScene(Room room, User user, ulong userGuid, int roomId, int targetRoomId, PBChannel channel, int handle, uint seq)
        {
            Msg_RL_ChangeSceneResult replyBuilder = new Msg_RL_ChangeSceneResult();
            EntityInfo info = user.Info;
            if (null != info) {
                replyBuilder.HP = info.Hp;
                replyBuilder.MP = info.Energy;
            }

            room.RemoveUserFromRoomThread(user);

            replyBuilder.UserGuid = userGuid;
            replyBuilder.RoomID = roomId;
            replyBuilder.TargetRoomID = targetRoomId;
            replyBuilder.Result = (int)SceneOperationResultEnum.Success;
            channel.Send(replyBuilder);
        }

        protected override void OnStart()
        {
            ActionNumPerTick = 1024;
            TickSleepTime = 10;
            LogSys.Log(LOG_TYPE.DEBUG, "thread {0} start.", cur_thread_id_);
        }

        protected override void OnTick()
        {
            try {
                long curTime = TimeUtility.GetLocalMilliseconds();
                if (m_LastTickTime != 0) {
                    long elapsedTickTime = curTime - m_LastTickTime;
                    if (elapsedTickTime > c_WarningTickTime) {
                        LogSys.Log(LOG_TYPE.MONITOR, "RoomThread Tick:{0}", elapsedTickTime);
                    }
                }
                m_LastTickTime = curTime;

                if (m_LastLogTime + 60000 < curTime) {
                    m_LastLogTime = curTime;

                    DebugPoolCount((string msg) => {
                        LogSys.Log(LOG_TYPE.INFO, "RoomThread.ActionQueue {0}, thread {1}", msg, cur_thread_id_);
                    });
                    LogSys.Log(LOG_TYPE.MONITOR, "RoomThread.ActionQueue Current Action {0}", this.CurActionNum);
                }

                long tick_interval_us = tick_interval_ * 1000;
                TimeSnapshot.Start();
                DoTick();
                long elapsedTime = TimeSnapshot.End();
                if (elapsedTime >= tick_interval_us) {
                    if (elapsedTime >= tick_interval_us * 2) {
                        LogSys.Log(LOG_TYPE.DEBUG, "*** Warning, RoomThread tick interval is {0} us !", elapsedTime);
                        foreach (Room room in active_room_) {
                            Scene scene = room.GetActiveScene();
                            if (null != scene) {
                                if (scene.SceneState == SceneState.Running) {
                                    SceneProfiler profiler = scene.SceneProfiler;
                                    LogSys.Log(LOG_TYPE.DEBUG, "{0}", profiler.GenerateLogString(scene.SceneResId, scene.GameTime.ElapseMilliseconds));
                                }
                            }
                        }
                    }
                    Thread.Sleep(0);
                } else {
                    Thread.Sleep((int)(tick_interval_ - elapsedTime / 1000));
                }
            } catch (Exception ex) {
                LogSys.Log(LOG_TYPE.ERROR, "Exception {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        protected override void OnQuit()
        {
        }

        private void DoTick()
        {
            foreach (Room rm in active_room_) {
                rm.Tick();
                if (rm.CanClose) {
                    unused_room_.Add(rm);
                }
            }
            foreach (Room rm in unused_room_) {
                room_mgr_.RemoveActiveRoom(rm.RoomID);
                active_room_.Remove(rm);
                rm.Destroy();
                room_pool_.FreeRoom(rm.LocalID);
            }
            unused_room_.Clear();
        }

        private User GetUserByGuid(ulong guid)
        {
            foreach (Room rm in active_room_) {
                return rm.GetUserByGuid(guid);
            }
            return null;
        }

        private bool ReplaceDroppedUser(ulong guid, ulong replacer, uint key)
        {
            foreach (Room rm in active_room_) {
                User us = rm.GetUserByGuid(guid);
                if (null != us && (int)UserControlState.UserDropped == us.UserControlState) {
                    return us.ReplaceDroppedUser(replacer, key);
                }
            }
            return false;
        }

        private Room GetRoomByID(int roomid)
        {
            foreach (Room rm in active_room_) {
                if (rm.RoomID == roomid) {
                    return rm;
                }
            }
            return null;
        }

        // thread control attribute------------------------------------------------
        private static uint thread_id_creator_ = 1;
        private uint cur_thread_id_;
        private uint tick_interval_;          // tick的间隔, 毫秒

        // room relative attribtes-------------
        private List<Room> active_room_;               // 已激活的房间ID列表
        private List<Room> unused_room_;
        private RoomPool room_pool_;
        private uint max_room_count_;
        private UserPool user_pool_;
        private ScenePool scene_pool_;

        private RoomManager room_mgr_;
        private Connector connector_;

        private const long c_WarningTickTime = 1000;
        private long m_LastTickTime = 0;
        private long m_LastLogTime = 0;
    }
}
