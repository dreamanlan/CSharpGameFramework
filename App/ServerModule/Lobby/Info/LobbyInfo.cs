using System;
using System.Collections.Generic;
using GameFramework;

namespace Lobby
{
    internal class SceneRoomsInfo
    {
        internal int m_SceneId = 0;
        internal List<int> m_RoomIds = new List<int>();
    }
    internal class SceneRoomInfo
    {
        internal int m_RoomIndex = 0;
        internal int m_UserCount = 0;
        internal int m_TotalUserCount = 0;
        internal int m_RoomId = 0;
        internal int m_SceneId = 0;
    }
    internal class UserServerInfo
    {
        internal long LastUpdateTime
        {
            get { return m_LastUpdateTime; }
            set { m_LastUpdateTime = value; }
        }
        internal int WorldId
        {
            get { return m_WorldId; }
            set { m_WorldId = value; }
        }
        internal int UserCount
        {
            get { return m_UserCount; }
            set { m_UserCount = value; }
        }

        internal void Reset()
        {
            m_LastUpdateTime = 0;

            m_WorldId = 0;
            m_UserCount = 0;
        }

        private int m_WorldId = 0;
        private int m_UserCount = 0;

        private long m_LastUpdateTime = 0;
    }
    internal class LobbyInfo
    {
        internal LobbyInfo()
        {
        }

        internal SortedDictionary<int, UserServerInfo> UserServerInfos
        {
            get { return m_LobbyServerInfos; }
        }

        internal SortedDictionary<string, RoomServerInfo> RoomServerInfos
        {
            get
            {
                return m_RoomServerInfos;
            }
        }

        internal SortedDictionary<int, RoomInfo> Rooms
        {
            get
            {
                return m_Rooms;
            }
        }

        internal SortedDictionary<int, SceneRoomsInfo> SceneInfos
        {
            get { return m_SceneInfos; }
        }

        internal void InitSceneRooms()
        {
            // 初始化野外场景房间
            MyDictionary<int, object> scenes = TableConfig.LevelProvider.Instance.LevelMgr.GetData();
            foreach (KeyValuePair<int, object> pair in scenes) {
                TableConfig.Level cfg = pair.Value as TableConfig.Level;
                if (null != cfg && cfg.type == (int)SceneTypeEnum.Room) {
                    SceneRoomsInfo fieldInfo;
                    if (!m_SceneInfos.TryGetValue(cfg.id, out fieldInfo)) {
                        fieldInfo = new SceneRoomsInfo();
                        fieldInfo.m_SceneId = cfg.id;
                        m_SceneInfos.Add(cfg.id, fieldInfo);
                    }
                    foreach (string roomServerName in cfg.RoomServer) {
                        for (int ix = 0; ix < cfg.ThreadCountPerScene; ++ix) {
                            for (int rix = 0; rix < cfg.RoomCountPerThread; ++rix) {
                                RoomInfo room = NewRoomInfo();
                                room.RoomId = ++m_CurRoomId;
                                room.SceneType = cfg.id;
                                room.RoomServerName = roomServerName;
                                room.TotalCount = cfg.MaxUserCount;

                                if (!m_Rooms.ContainsKey(room.RoomId)) {
                                    m_Rooms.Add(room.RoomId, room);
                                } else {
                                    m_Rooms[room.RoomId] = room;
                                }
                                if (!fieldInfo.m_RoomIds.Contains(room.RoomId)) {
                                    fieldInfo.m_RoomIds.Add(room.RoomId);
                                }
                                room.Creator = 0;
                            }
                        }
                    }
                }
            }
        }

        internal RoomInfo GetRoomByID(int roomId)
        {
            RoomInfo info = null;
            m_Rooms.TryGetValue(roomId, out info);
            return info;
        }

        internal int FindSceneRoom(int sceneId)
        {
            SceneRoomsInfo info;
            if (m_SceneInfos.TryGetValue(sceneId, out info)) {
                foreach (int id in info.m_RoomIds) {
                    RoomInfo room = GetRoomByID(id);
                    if (null != room && room.UserCount < room.TotalCount && m_RoomServerInfos.ContainsKey(room.RoomServerName)) {
                        return id;
                    }
                }
            }
            return -1;
        }
        internal SceneRoomInfo GetSceneRoomInfo(int sceneId, int roomId)
        {
            SceneRoomInfo ret = null;
            SceneRoomsInfo info;
            if (m_SceneInfos.TryGetValue(sceneId, out info)) {
                ret = new SceneRoomInfo();
                ret.m_SceneId = sceneId;
                ret.m_RoomId = roomId;
                int ix = info.m_RoomIds.IndexOf(roomId);
                ret.m_RoomIndex = ix + 1;
                RoomInfo room = GetRoomByID(roomId);
                if (null != room) {
                    ret.m_UserCount = room.UserCount;
                    ret.m_TotalUserCount = room.TotalCount;
                }
            }
            return ret;
        }
        internal List<SceneRoomInfo> GetSceneRoomList(int sceneId)
        {
            List<SceneRoomInfo> infos = new List<SceneRoomInfo>();
            SceneRoomsInfo info;
            if (m_SceneInfos.TryGetValue(sceneId, out info)) {
                int ct = info.m_RoomIds.Count;
                for (int i = 0; i < ct; ++i) {
                    int id = info.m_RoomIds[i];
                    RoomInfo room = GetRoomByID(id);
                    if (null != room && m_RoomServerInfos.ContainsKey(room.RoomServerName)) {
                        SceneRoomInfo tinfo = new SceneRoomInfo();
                        tinfo.m_SceneId = sceneId;
                        tinfo.m_RoomId = id;
                        tinfo.m_RoomIndex = i + 1;
                        tinfo.m_UserCount = room.UserCount;
                        tinfo.m_TotalUserCount = room.TotalCount;
                        infos.Add(tinfo);
                    }
                }
            }
            return infos;
        }

        internal void Tick()
        {
            Queue<RoomInfo> recycles = new Queue<RoomInfo>();
            foreach (KeyValuePair<int, RoomInfo> pair in m_Rooms) {
                RoomInfo room = pair.Value;
                room.Tick();
                if (room.CurrentState == RoomState.Close) {
                    recycles.Enqueue(room);
                }
            }
            while (recycles.Count > 0) {
                RoomInfo room = recycles.Dequeue();
                m_Rooms.Remove(room.RoomId);
                if (room.IsCustomRoom)
                    m_CustomRooms.Remove(room.RoomId);
                RecycleRoomInfo(room);
            }
        }
        
        private RoomInfo NewRoomInfo()
        {
            RoomInfo room = null;
            if (m_UnusedRoomInfos.Count > 0) {
                room = m_UnusedRoomInfos.Dequeue();
            } else {
                room = new RoomInfo();
                room.LobbyInfo = this;
            }
            room.CurrentState = RoomState.Prepare;
            return room;
        }

        private void RecycleRoomInfo(RoomInfo room)
        {
            room.Reset();
            m_UnusedRoomInfos.Enqueue(room);
        }

        private SortedDictionary<int, UserServerInfo> m_LobbyServerInfos = new SortedDictionary<int, UserServerInfo>();
        private SortedDictionary<string, RoomServerInfo> m_RoomServerInfos = new SortedDictionary<string, RoomServerInfo>();
        private SortedDictionary<int, RoomInfo> m_Rooms = new SortedDictionary<int, RoomInfo>();
        private SortedDictionary<int, SceneRoomsInfo> m_SceneInfos = new SortedDictionary<int, SceneRoomsInfo>();
        private List<int> m_CustomRooms = new List<int>();
        private int m_CurRoomId = 0;

        private Queue<RoomInfo> m_UnusedRoomInfos = new Queue<RoomInfo>();
    }
}
