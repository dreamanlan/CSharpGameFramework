using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpCenterClient;
using GameFrameworkMessage;
using System.Collections.Generic;
using GameFramework;
using LitJson;

namespace Lobby
{
    /// <summary>
    /// Room logic thread. Handle various logics after players form teams in the lobby.
    /// </summary>
    /// <remarks>
    /// Other threads should not call such methods directly, but should initiate calls through QueueAction.
    /// </remarks>
    internal sealed class RoomProcessThread : MyServerThread
    {
        internal RoomProcessThread()
        {
        }
        internal void Init()
        {
            m_LobbyInfo.InitSceneRooms();
        }
        internal void UpdateUserServerInfo(int worldId, int userCount)
        {
            UserServerInfo info;
            if (!m_LobbyInfo.UserServerInfos.TryGetValue(worldId, out info)) {
                info = new UserServerInfo();
                m_LobbyInfo.UserServerInfos.Add(worldId, info);
            }
            info.LastUpdateTime = TimeUtility.GetLocalMilliseconds();
            info.WorldId = worldId;
            info.UserCount = userCount;

            LogSys.Log(ServerLogType.MONITOR, "UpdateLobbyServerInfo, world id:{0}, user count:{1}", worldId, userCount);
        }
        internal void RegisterRoomServer(RoomServerInfo info)
        {
            if (!m_LobbyInfo.RoomServerInfos.ContainsKey(info.RoomServerName)) {
                m_LobbyInfo.RoomServerInfos.Add(info.RoomServerName, info);
            } else {
                RoomServerInfo info_ = m_LobbyInfo.RoomServerInfos[info.RoomServerName];
                info_.RoomServerName = info.RoomServerName;
                info_.ServerIp = info.ServerIp;
                info_.ServerPort = info.ServerPort;
                info_.MaxRoomNum = info.MaxRoomNum;
            }
            Msg_LR_ReplyRegisterRoomServer resultBuilder = new Msg_LR_ReplyRegisterRoomServer();
            resultBuilder.IsOk = true;
            LobbyServer.Instance.RoomSvrChannel.Send(info.RoomServerName, resultBuilder);
            LogSys.Log(ServerLogType.DEBUG, "RegisterRoomServer,name:{0},ip:{1},port:{2},max room num:{3}", info.RoomServerName, info.ServerIp, info.ServerPort, info.MaxRoomNum);
        }
        internal void UpdateRoomServerInfo(RoomServerInfo info)
        {
            RoomServerInfo info_;
            if (!m_LobbyInfo.RoomServerInfos.TryGetValue(info.RoomServerName, out info_))
                m_LobbyInfo.RoomServerInfos.Add(info.RoomServerName, info);
            else {
                info_.IdleRoomNum = info.IdleRoomNum;
                info_.UserNum = info.UserNum;
            }

            //LogSys.Log(LOG_TYPE.DEBUG, "UpdateRoomServerInfo,name:{0},idle room num:{1},cur user num:{2}", info.RoomServerName, info.IdleRoomNum, info.UserNum);
        }

        //===================================================================
        //Field line related processing
        //===================================================================
        internal void RequestSceneRoomInfo(ulong guid)
        {
            UserProcessScheduler dataProcess = LobbyServer.Instance.UserProcessScheduler;
            UserInfo user = dataProcess.GetUserInfo(guid);
            if (null != user) {
                if (user.CurrentState == UserState.Room) {
                    RoomInfo room = user.Room;
                    if (null != room) {
                        SceneRoomInfo info = m_LobbyInfo.GetSceneRoomInfo(room.SceneType, room.RoomId);
                        if (null != info) {
                            string chapterName;
                            TableConfig.Level cfg = TableConfig.LevelProvider.Instance.GetLevel(info.m_SceneId);
                            if (null != cfg) {
                                chapterName = "unknown";
                            } else {
                                chapterName = "unknown";
                            }
                            LobbyServer.Instance.SendStoryMessage(user, "onroominfo", info.m_RoomIndex, info.m_UserCount, info.m_TotalUserCount, info.m_RoomId, info.m_SceneId, chapterName);
                        }
                    }
                }
            }
        }
        internal void RequestSceneRoomList(ulong guid)
        {
            UserProcessScheduler dataProcess = LobbyServer.Instance.UserProcessScheduler;
            UserInfo user = dataProcess.GetUserInfo(guid);
            if (null != user) {
                if (user.CurrentState == UserState.Room) {
                    RoomInfo room = user.Room;
                    if (null != room) {
                        List<SceneRoomInfo> list = m_LobbyInfo.GetSceneRoomList(room.SceneType);
                        JsonData jsonList = new JsonData();
                        int ct = list.Count;
                        for (int i = 0; i < ct; ++i) {
                            JsonData json = new JsonData();
                            json.Add(list[i].m_RoomIndex);
                            json.Add(list[i].m_UserCount);
                            json.Add(list[i].m_TotalUserCount);
                            json.Add(list[i].m_RoomId);
                            json.Add(list[i].m_SceneId);
                            jsonList.Add(json);
                        }
                        if (ct > 0) {
                            LobbyServer.Instance.SendStoryMessage(user, "onroomlist", JsonMapper.ToJson(jsonList));
                        }
                    }
                }
            }
        }

        internal void RequestEnterScene(ulong guid, int sceneId, int wantRoomId, int fromSceneId)
        {
            UserInfo info = LobbyServer.Instance.UserProcessScheduler.GetUserInfo(guid);
            if (info != null) {
                float enterX = 50, enterY = 50;
                /*
                int transportId = TransportConfigProvider.GenTransportId(fromSceneId,sceneId);
                TransportConfig cfg = TransportConfigProvider.Instance.GetTransportConfig(transportId);
                if (null != cfg) {
                    enterX = cfg.m_EnterX;
                    enterY = cfg.m_EnterY;
                }
                */
                if (wantRoomId <= 0) {
                    int roomId = m_LobbyInfo.FindSceneRoom(sceneId);
                    if (roomId > 0) {
                        RequestEnterSceneRoom(info, roomId, 0, 0, enterX, enterY);
                    }
                } else {
                    RoomInfo roomUserMgr = m_LobbyInfo.GetRoomByID(wantRoomId);
                    if (null != roomUserMgr && roomUserMgr.UserCount < roomUserMgr.TotalCount) {
                        RequestEnterSceneRoom(info, wantRoomId, 0, 0, enterX, enterY);
                    } else {
                        LobbyServer.Instance.HighlightPrompt(info, 42);//Failed to enter the game, please try again later.
                    }
                }
            }
        }
        internal void RequestChangeSceneRoom(ulong guid, int sceneId, int wantRoomId)
        {
            UserInfo info = LobbyServer.Instance.UserProcessScheduler.GetUserInfo(guid);
            RoomInfo targetRoom = null;
            if (wantRoomId <= 0) {
                int roomId = m_LobbyInfo.FindSceneRoom(sceneId);
                if (roomId > 0) {
                    targetRoom = m_LobbyInfo.GetRoomByID(roomId);
                }
            } else {
                targetRoom = m_LobbyInfo.GetRoomByID(wantRoomId);
            }
            if (info != null && null != info.Room && null != targetRoom) {
                RoomInfo curRoom = info.Room;
                if (curRoom.RoomId != targetRoom.RoomId && targetRoom.UserCount < targetRoom.TotalCount) {
                    Msg_LR_ChangeScene msg = new Msg_LR_ChangeScene();
                    msg.UserGuid = guid;
                    msg.RoomId = curRoom.RoomId;
                    msg.TargetRoomId = targetRoom.RoomId;

                    LobbyServer.Instance.RoomSvrChannel.Send(curRoom.RoomServerName, msg);
                } else {
                    LobbyServer.Instance.HighlightPrompt(info, 42);//Failed to enter the game, please try again later.
                }
            } else {
                LobbyServer.Instance.HighlightPrompt(info, 42);//Failed to enter the game, please try again later.
            }
        }

        internal void QuitRoom(ulong guid, bool is_quit_room, ulong srcHandle)
        {
            UserInfo user = LobbyServer.Instance.UserProcessScheduler.GetUserInfo(guid);
            if (user != null) {
                if (user.CurrentState == UserState.Room) {
                    RoomInfo room = user.Room;
                    if (null != room) {
                        Msg_LR_UserQuit msg = new Msg_LR_UserQuit();
                        msg.UserGuid = guid;
                        msg.RoomId = room.RoomId;
                        LobbyServer.Instance.RoomSvrChannel.Send(room.RoomServerName, msg);
                    }
                } else if (null != user.Room) {
                    user.Room.DelUsers(guid);
                    user.ResetRoomInfo();
                    user.CurrentState = UserState.Online;
                    user.LeftLife = UserInfo.c_LifeTimeWaitOffline;
                }
                LogSys.Log(ServerLogType.INFO, "QuitRoom Guid {0} state {1}", guid, user.CurrentState);
            } else {
                Msg_BL_QueryUserStateResult builder = new Msg_BL_QueryUserStateResult();
                builder.Guid = guid;
                builder.State = (int)UserState.DropOrOffline;
                LobbyServer.Instance.UserChannel.Send(srcHandle, builder);
            }
        }
        internal void HandleBuyLife(ulong guid)
        {
            UserProcessScheduler scheduler = LobbyServer.Instance.UserProcessScheduler;
            if (null == scheduler)
                return;
            // Respond to player requests for resurrection
            UserInfo user = scheduler.GetUserInfo(guid);
            if (null != user) {
                RoomInfo room = user.Room;
                if (null != room) {
                    Msg_LR_UserReLive resultBuilder = new Msg_LR_UserReLive();
                    resultBuilder.UserGuid = guid;
                    resultBuilder.RoomId = user.CurrentRoomID;
                    LobbyServer.Instance.RoomSvrChannel.Send(room.RoomServerName, resultBuilder);

                    LogSys.Log(ServerLogType.INFO, "BuyLife Guid {0}", guid);
                }
            }
        }
        internal void UserOffline(ulong guid, ulong srcHandle)
        {
            UserInfo user = LobbyServer.Instance.UserProcessScheduler.GetUserInfo(guid);
            if (user != null) {
                if (null != user.Room) {
                    Msg_LR_UserQuit msg = new Msg_LR_UserQuit();
                    msg.UserGuid = guid;
                    msg.RoomId = user.Room.RoomId;
                    LobbyServer.Instance.RoomSvrChannel.Send(user.Room.RoomServerName, msg);

                    Msg_BL_UserOffline builder = new Msg_BL_UserOffline();
                    builder.Guid = guid;
                    LobbyServer.Instance.UserChannel.Send(srcHandle, builder);

                    LogSys.Log(ServerLogType.INFO, "UserOffline, guid:{0}", guid);
                }
            }
        }
        internal void UserRelogin(ulong guid, ulong srcHandle)
        {
            UserInfo user = LobbyServer.Instance.UserProcessScheduler.GetUserInfo(guid);
            if (user != null) {
                if (user.CurrentState == UserState.Room) {
                    RoomInfo room = m_LobbyInfo.GetRoomByID(user.CurrentRoomID);
                    if (room != null) {
                        //Send a message to RoomServer to re-enter the room
                        Msg_LR_ReconnectUser urBuilder = new Msg_LR_ReconnectUser();
                        urBuilder.UserGuid = guid;
                        urBuilder.RoomId = user.CurrentRoomID;
                        LobbyServer.Instance.RoomSvrChannel.Send(room.RoomServerName, urBuilder);

                        LogSys.Log(ServerLogType.INFO, "User Restart GameClient , guid:{0}", guid);
                    } else {
                        //The room has been closed
                        user.ResetRoomInfo();
                        user.CurrentState = UserState.Online;

                        Msg_BL_QueryUserStateResult builder = new Msg_BL_QueryUserStateResult();
                        builder.Guid = guid;
                        builder.State = (int)UserState.Online;
                        LobbyServer.Instance.UserChannel.Send(srcHandle, builder);
                    }
                }
            } else {
                Msg_BL_QueryUserStateResult builder = new Msg_BL_QueryUserStateResult();
                builder.Guid = guid;
                builder.State = (int)UserState.DropOrOffline;
                LobbyServer.Instance.UserChannel.Send(srcHandle, builder);
            }
        }

        internal void HandleRoomStoryMessage(Msg_LRL_StoryMessage msg_)
        {
            UserProcessScheduler dataProcess = LobbyServer.Instance.UserProcessScheduler;
            UserInfo user = dataProcess.GetUserInfo(msg_.UserGuid);
            if (null != user) {
                RoomInfo info = m_LobbyInfo.GetRoomByID(user.CurrentRoomID);
                if (null != info) {
                    msg_.RoomId = user.CurrentRoomID;
                    LobbyServer.Instance.RoomSvrChannel.Send(info.RoomServerName, msg_);
                }
            }
        }
        //===================================================================
        //The following is the message communication with RoomServer
        //Respond to room creation feedback messages sent by RoomServer
        internal void OnEnterSceneResult(ulong userGuid, int roomID, int result)
        {
            UserInfo user = LobbyServer.Instance.UserProcessScheduler.GetUserInfo(userGuid);
            if (user == null) { return; }
            if ((int)SceneOperationResultEnum.Success == result) {
                user.CurrentState = UserState.Room;
                user.CurrentRoomID = roomID;
                RoomInfo room = m_LobbyInfo.GetRoomByID(roomID);
                if (null != room) {
                    RoomServerInfo svrInfo;
                    if (m_LobbyInfo.RoomServerInfos.TryGetValue(room.RoomServerName, out svrInfo)) {
                        if (null != svrInfo) {
                            NodeMessage enterSceneResultMsg = new NodeMessage(LobbyMessageDefine.EnterSceneResult, user.Guid);
                            GameFrameworkMessage.EnterSceneResult protoData = new GameFrameworkMessage.EnterSceneResult();
                            protoData.server_ip = svrInfo.ServerIp;
                            protoData.server_port = svrInfo.ServerPort;
                            protoData.key = user.Key;
                            protoData.camp_id = user.CampId;
                            protoData.scene_type = room.SceneType;
                            protoData.result = (int)GeneralOperationResult.LC_Succeed;
                            protoData.prime = Helper.Random.Next(2, 21);

                            enterSceneResultMsg.m_ProtoData = protoData;
                            LobbyServer.Instance.TransmitToWorld(user, enterSceneResultMsg);

                            LogSys.Log(ServerLogType.INFO, "user enter field success! guid {0} room {1} result {2}",
                              userGuid, roomID, (SceneOperationResultEnum)result);
                        } else {
                            LobbyServer.Instance.HighlightPrompt(user, 42);//Failed to enter the game, please try again later.
                        }
                    } else {
                        LobbyServer.Instance.HighlightPrompt(user, 42);//Failed to enter the game, please try again later.
                    }
                } else {
                    LobbyServer.Instance.HighlightPrompt(user, 42);//Failed to enter the game, please try again later.
                }
            } else {
                RoomInfo curRoom = user.Room;
                if (null != curRoom) {
                    curRoom.DelUsers(userGuid);
                }
                user.CurrentState = UserState.Online;

                LogSys.Log(ServerLogType.INFO, "user enter field failed! guid {0} room {1} result {2}",
                  userGuid, roomID, (SceneOperationResultEnum)result);

                LobbyServer.Instance.HighlightPrompt(user, 42);//Failed to enter the game, please try again later.
            }
        }
        internal void OnChangeScene(IList<ulong> userGuids, int sceneID)
        {
            bool failed = true;
            RoomInfo targetRoom = null;
            int roomId = m_LobbyInfo.FindSceneRoom(sceneID);
            if (roomId > 0) {
                targetRoom = m_LobbyInfo.GetRoomByID(roomId);
                if (null != targetRoom && targetRoom.UserCount + userGuids.Count <= targetRoom.TotalCount) {
                    for (int i = 0; i < userGuids.Count; ++i) {
                        ulong userGuid = userGuids[i];
                        UserInfo user = LobbyServer.Instance.UserProcessScheduler.GetUserInfo(userGuid);
                        if (user == null) { continue; }
                        if (null != user.Room) {
                            RoomInfo curRoom = user.Room;
                            if (curRoom.RoomId != targetRoom.RoomId) {
                                Msg_LR_ChangeScene msg = new Msg_LR_ChangeScene();
                                msg.UserGuid = userGuid;
                                msg.RoomId = curRoom.RoomId;
                                msg.TargetRoomId = targetRoom.RoomId;

                                LobbyServer.Instance.RoomSvrChannel.Send(curRoom.RoomServerName, msg);
                                LogSys.Log(ServerLogType.INFO, "User change field ! , guid:{0} room:{1} target room:{2}", userGuid, curRoom.RoomId, targetRoom.RoomId);
                                failed = false;
                            }
                        }
                    }
                }
            }
            if (failed) {
                for (int i = 0; i < userGuids.Count; ++i) {
                    ulong userGuid = userGuids[i];
                    UserInfo user = LobbyServer.Instance.UserProcessScheduler.GetUserInfo(userGuid);
                    if (null != user) {
                        LobbyServer.Instance.HighlightPrompt(user, 42);//Failed to enter the game, please try again later.
                    }
                }
            }
        }
        internal void OnChangeSceneResult(ulong userGuid, int roomID, int targetRoomID, int result, int hp, int mp)
        {
            UserInfo user = LobbyServer.Instance.UserProcessScheduler.GetUserInfo(userGuid);
            if (user == null) { return; }
            if ((int)SceneOperationResultEnum.Success == result) {
                RoomInfo room = m_LobbyInfo.GetRoomByID(roomID);
                RoomInfo targetRoom = m_LobbyInfo.GetRoomByID(targetRoomID);
                if (null != room && null != targetRoom) {
                    int fromSceneId = room.SceneType;
                    int targetSceneId = targetRoom.SceneType;
                    float enterX = 50, enterY = 50;
                    /*
                    int transportId = TransportConfigProvider.GenTransportId(fromSceneId, targetSceneId);
                    TransportConfig cfg = TransportConfigProvider.Instance.GetTransportConfig(transportId);
                    if (null != cfg) {
                        enterX = cfg.m_EnterX;
                        enterY = cfg.m_EnterY;
                    }
                    */
                    room.DelUsers(userGuid);
                    if (room.RoomServerName == targetRoom.RoomServerName) {
                        targetRoom.AddUsers(user.CampId, userGuid);
                        user.CurrentState = UserState.Room;

                        Msg_BL_UserChangeScene msg = new Msg_BL_UserChangeScene();
                        msg.Guid = userGuid;
                        msg.SceneId = targetRoom.SceneType;
                        LobbyServer.Instance.UserChannel.Send(user.UserSvrName, msg);
                    } else {
                        user.CurrentState = UserState.Online;
                        RequestEnterSceneRoom(user, targetRoomID, hp, mp, enterX, enterY);
                    }
                    LogSys.Log(ServerLogType.INFO, "User change field success ! , guid:{0} room:{1} target room:{2} result:{3}", userGuid, roomID, targetRoomID, (SceneOperationResultEnum)result);
                } else {
                    LobbyServer.Instance.HighlightPrompt(user, 42);//Failed to enter the game, please try again later.
                }
            } else {
                LogSys.Log(ServerLogType.INFO, "User change field failed ! guid:{0} room:{1} target room:{2} result:{3}", userGuid, roomID, targetRoomID, (SceneOperationResultEnum)result);

                LobbyServer.Instance.HighlightPrompt(user, 42);//Failed to enter the game, please try again later.
            }
        }
        internal void OnActiveScene(ulong[] guids, int sceneId)
        {
            Msg_LR_ActiveScene msg = new Msg_LR_ActiveScene();
            msg.UserGuids = guids;
            msg.RoomId = m_LobbyInfo.CreateAutoRoom(guids, sceneId);
            msg.SceneId = sceneId;

            RoomInfo room = m_LobbyInfo.GetRoomByID(msg.RoomId);
            if (null != room) {
                LobbyServer.Instance.RoomSvrChannel.Send(room.RoomServerName, msg);
            }
        }
        internal void OnActiveSceneResult(ulong[] guids, int roomID, int result)
        {
            //When the dungeon is successfully opened, the scene switching process for each player is initiated (note that the dungeon is switched)
            if (result == (int)SceneOperationResultEnum.Success) {
                for (int i = 0; i < guids.Length; ++i) {
                    ulong guid = guids[i];
                    RequestChangeSceneRoom(guid, 0, roomID);
                }
            }
        }
        //Respond to feedback messages from RoomServer players reconnecting to the room
        internal void OnReplyReconnectUser(ulong userGuid, int roomID, int result)
        {
            UserInfo user = LobbyServer.Instance.UserProcessScheduler.GetUserInfo(userGuid);
            if (user == null) { return; }
            switch (result) {
                case (int)Msg_RL_ReplyReconnectUser.ReconnectResultEnum.Drop: {
                        user.CurrentState = UserState.Room;
                        RoomInfo room = m_LobbyInfo.GetRoomByID(roomID);
                        if (null != room) {
                            RoomServerInfo svrInfo;
                            if (m_LobbyInfo.RoomServerInfos.TryGetValue(room.RoomServerName, out svrInfo)) {
                                if (null != svrInfo) {
                                    NodeMessage startGameResultMsg = new NodeMessage(LobbyMessageDefine.EnterSceneResult, user.Guid);
                                    GameFrameworkMessage.EnterSceneResult protoData = new GameFrameworkMessage.EnterSceneResult();
                                    protoData.server_ip = svrInfo.ServerIp;
                                    protoData.server_port = svrInfo.ServerPort;
                                    protoData.key = user.Key;
                                    protoData.camp_id = user.CampId;
                                    protoData.scene_type = room.SceneType;
                                    protoData.result = (int)GeneralOperationResult.LC_Succeed;
                                    protoData.prime = Helper.Random.Next(2, 21);

                                    startGameResultMsg.m_ProtoData = protoData;
                                    LobbyServer.Instance.TransmitToWorld(user, startGameResultMsg);
                                    //Re-entered the room successfully
                                    LogSys.Log(ServerLogType.INFO, "user reconnected roomServer success, guid {0}", userGuid);
                                }
                            }
                        }
                    }
                    break;
                case (int)Msg_RL_ReplyReconnectUser.ReconnectResultEnum.NotExist: {
                        //Does not exist, execute the scene process
                        RequestEnterScene(userGuid, user.SceneId, roomID, 0);
                        LogSys.Log(ServerLogType.INFO, "user reconnected roomserver, not exist, request enter scene ! guid {0}", userGuid);
                    }
                    break;
                case (int)Msg_RL_ReplyReconnectUser.ReconnectResultEnum.Online: {
                        NodeMessage startGameResultMsg = new NodeMessage(LobbyMessageDefine.EnterSceneResult, user.Guid);
                        GameFrameworkMessage.EnterSceneResult protoData = new GameFrameworkMessage.EnterSceneResult();
                        protoData.result = (int)GeneralOperationResult.LC_Failed;

                        startGameResultMsg.m_ProtoData = protoData;
                        LobbyServer.Instance.TransmitToWorld(user, startGameResultMsg);
                        //Players in the room are still connected and online, but reconnection fails.
                        LogSys.Log(ServerLogType.INFO, "user reconnected roomserver, user already online, guid {0}", userGuid);
                    }
                    break;
            }
        }
        //Respond to the message sent by RoomServer that players in the room are offline
        internal void OnRoomUserDrop(int roomid, ulong guid, bool isBattleEnd, Msg_RL_UserDrop originalMsg)
        {
            UserInfo user = LobbyServer.Instance.UserProcessScheduler.GetUserInfo(guid);
            if (user != null) {
                if (isBattleEnd) {
                    user.CurrentRoomID = 0;
                    user.CurrentState = UserState.Room == user.CurrentState ? UserState.Online : user.CurrentState;
                } else {
                    user.CurrentRoomID = roomid;
                }
                RoomInfo curRoom = user.Room;
                RoomInfo oldRoom = m_LobbyInfo.GetRoomByID(roomid);
                if (null != oldRoom && oldRoom != curRoom) {
                    Msg_LR_UserQuit msg = new Msg_LR_UserQuit();
                    msg.UserGuid = guid;
                    msg.RoomId = roomid;
                    LobbyServer.Instance.RoomSvrChannel.Send(oldRoom.RoomServerName, msg);
                }
                LobbyServer.Instance.ForwardToWorld(user.UserSvrName, originalMsg);

                LogSys.Log(ServerLogType.INFO, "RoomServer User Drop! Guid {0} State {1} IsEnd {2}", guid, user.CurrentState, isBattleEnd);
            } else {
                RoomInfo oldRoom = m_LobbyInfo.GetRoomByID(roomid);
                if (null != oldRoom) {
                    Msg_LR_UserQuit msg = new Msg_LR_UserQuit();
                    msg.UserGuid = guid;
                    msg.RoomId = roomid;
                    LobbyServer.Instance.RoomSvrChannel.Send(oldRoom.RoomServerName, msg);
                }

                LogSys.Log(ServerLogType.INFO, "RoomServer User Drop! Guid {0} State Offline IsEnd {1}", guid, isBattleEnd);
            }
        }
        internal void OnRoomUserQuit(int roomid, ulong guid, Msg_RL_UserQuit originalMsg)
        {
            UserInfo user = LobbyServer.Instance.UserProcessScheduler.GetUserInfo(guid);
            if (user != null) {
                RoomInfo room = user.Room;
                if (null != room) {
                    if (room.RoomId == roomid) {
                        room.DelUsers(guid);
                        user.ResetRoomInfo();
                        user.CurrentRoomID = 0;
                        user.CurrentState = UserState.Room == user.CurrentState ? UserState.Online : user.CurrentState;
                    }
                } else {
                    user.CurrentRoomID = 0;
                    user.CurrentState = UserState.Room == user.CurrentState ? UserState.Online : user.CurrentState;
                }

                LobbyServer.Instance.ForwardToWorld(user.UserSvrName, originalMsg);

                LogSys.Log(ServerLogType.INFO, "RoomServer User Quit Guid {0} State {1}", guid, user.CurrentState);
            }
        }
        //Pickup related
        internal void OnPickMoney(Msg_RL_PickMoney msg)
        {
            UserInfo user = LobbyServer.Instance.UserProcessScheduler.GetUserInfo(msg.UserGuid);
            if (user != null) {
                LobbyServer.Instance.ForwardToWorld(user.UserSvrName, msg);
            }
        }
        internal void OnPickItem(Msg_RL_PickItem msg)
        {
            UserProcessScheduler dataProcess = LobbyServer.Instance.UserProcessScheduler;
            UserInfo user = dataProcess.GetUserInfo(msg.UserGuid);
            if (user != null && null != user.Room) {
                RoomInfo room = user.Room;
                if (room.RoomId == msg.RoomId) {
                    msg.RoomSvrName = room.RoomServerName;
                    LobbyServer.Instance.ForwardToWorld(user.UserSvrName, msg);
                }
            }
        }
        private void RequestEnterSceneRoom(UserInfo info, int roomId, int hp, int mp, float x, float y)
        {
            RoomInfo room = m_LobbyInfo.GetRoomByID(roomId);
            if (null != room) {
                int campId = (int)CampIdEnum.Blue;

                room.AddUsers(campId, info.Guid);
                Msg_LR_EnterScene enterSceneMsg = new Msg_LR_EnterScene();
                enterSceneMsg.UserGuid = info.Guid;
                enterSceneMsg.RoomId = roomId;

                Msg_LR_RoomUserInfo ruiBuilder = info.RoomUserInfo;
                ruiBuilder.Key = info.Key;
                ruiBuilder.Camp = info.CampId;
                if (hp > 0 && mp > 0) {
                    enterSceneMsg.HP = hp;
                    enterSceneMsg.MP = mp;
                }
                if (x > 0 && y > 0) {
                    ruiBuilder.EnterX = x;
                    ruiBuilder.EnterY = y;
                }
                enterSceneMsg.UserInfo = ruiBuilder;

                LobbyServer.Instance.RoomSvrChannel.Send(room.RoomServerName, enterSceneMsg);

                LogSys.Log(ServerLogType.DEBUG, "Request enter field, user {0} room {1}", info.Guid, roomId);
            }
        }
        protected override void OnStart()
        {
            TickSleepTime = 10;
            ActionNumPerTick = 4096;
        }
        protected override void OnTick()
        {
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (m_LastTickTime != 0) {
                long elapsedTickTime = curTime - m_LastTickTime;
                if (elapsedTickTime > c_WarningTickTime) {
                    LogSys.Log(ServerLogType.MONITOR, "RoomProcessThread Tick:{0}", elapsedTickTime);
                }
            }
            m_LastTickTime = curTime;

            if (m_LastLogTime + 60000 < curTime) {
                m_LastLogTime = curTime;

                DebugPoolCount((string msg) => {
                    LogSys.Log(ServerLogType.INFO, "RoomProcessThread.ActionQueue {0}", msg);
                });
                LogSys.Log(ServerLogType.MONITOR, "RoomProcessThread.ActionQueue Current Action {0}", this.CurActionNum);

                int gameRoomCount = 0;
                int gameUserCount = 0;
                foreach (KeyValuePair<int, RoomInfo> pair in m_LobbyInfo.Rooms) {
                    var room = pair.Value;
                    if (room.CurrentState == RoomState.Game) {
                        gameRoomCount++;
                        gameUserCount += room.UserCount;
                    }
                }
                LogSys.Log(ServerLogType.MONITOR, "Lobby Game Room Count:{0}, GameUserCount:{1}", gameRoomCount, gameUserCount);
            }

            m_LobbyInfo.Tick();
        }

        public LobbyInfo GetLobbyInfo
        {
            get { return m_LobbyInfo; }
        }

        private const int c_MaxMemberCount = 5;
        private LobbyInfo m_LobbyInfo = new LobbyInfo();

        private const long c_WarningTickTime = 1000;
        private long m_LastTickTime = 0;
        private long m_LastLogTime = 0;
    }
}

