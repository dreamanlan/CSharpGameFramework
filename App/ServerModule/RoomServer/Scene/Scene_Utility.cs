using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFrameworkMessage;
using ScriptRuntime;

namespace ScriptableFramework
{
    public sealed partial class Scene
    {
        private void LoadObjects()
        {
            LogSys.Log(ServerLogType.DEBUG, "Scene {0} start Running.", m_SceneResId);
            m_GameTime.Start();
            m_SceneContext.StartTime = m_GameTime.StartTime;

            for (int campId = (int)CampIdEnum.Friendly; campId <= (int)CampIdEnum.Red; ++campId) {
                int key = this.SceneResId * 10 + campId;
                List<TableConfig.LevelMonster> monsters;
                if (TableConfig.LevelMonsterProvider.Instance.TryGetValue(key, out monsters)) {
                    for (int i = 0; i < monsters.Count; ++i) {
                        TableConfig.LevelMonster monster = monsters[i];
                        if (null != monster) {
                            int unitId = campId * 10000 + i;
                            int objId = CreateEntity(unitId, monster.x, 0.0f, monster.y, monster.dir, campId, monster.actorID, monster.aiLogic, monster.aiParams.ToArray());
                            EntityInfo npc = GetEntityById(objId);
                            if (null != npc) {
                                npc.IsPassive = monster.passive;
                                npc.LevelMonsterData = monster;
                                npc.Level = monster.level;
                            }
                        }
                    }
                }
            }

            ServerStorySystem.ThreadInitMask();
            m_StorySystem.ClearStoryInstancePool();
            m_StorySystem.LoadSceneStories();
            m_StorySystem.StartStory("local_main");
            m_StorySystem.StartStory("story_main");
        }
        
        private void SyncSceneObjectsToUser(User user)
        {
            if (null != user) {
                EntityInfo userInfo = user.Info;
                RoomUserManager roomUserMgr = GetRoomUserManager();
                if (null != userInfo && null != roomUserMgr && null != roomUserMgr.ActiveScene) {
                    for (LinkedListNode<EntityInfo> linkNode = EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                        EntityInfo npc = linkNode.Value;
                        if (null != npc) {
                            Msg_RC_CreateNpc bder = DataSyncUtility.BuildCreateNpcMessage(npc);
                            user.SendMessage(RoomMessageDefine.Msg_RC_CreateNpc, bder);

                            Msg_RC_SyncProperty msg = DataSyncUtility.BuildSyncPropertyMessage(npc);
                            user.SendMessage(RoomMessageDefine.Msg_RC_SyncProperty, msg);
                        }
                    }
                }
            }
        }

        private void SyncUserObjectToOtherUsers(User user)
        {
            if (null != user) {
                EntityInfo userInfo = user.Info;
                Msg_RC_CreateNpc bder = DataSyncUtility.BuildCreateNpcMessage(userInfo);
                Msg_RC_SyncProperty msg = DataSyncUtility.BuildSyncPropertyMessage(userInfo);
                RoomUserManager roomUserMgr = GetRoomUserManager();
                if (null != userInfo && null != roomUserMgr && null != roomUserMgr.ActiveScene) {
                    for (LinkedListNode<EntityInfo> linkNode = EntityManager.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                        EntityInfo npc = linkNode.Value;
                        if (null != npc && npc != userInfo) {
                            User other = npc.CustomData as User;
                            if (null != other) {
                                other.SendMessage(RoomMessageDefine.Msg_RC_CreateNpc, bder);
                                other.SendMessage(RoomMessageDefine.Msg_RC_SyncProperty, msg);
                            }
                        }
                    }
                }
            }
        }

        private void UpdateKillCount(EntityInfo npc)
        {
            if (null == npc)
                return;
            EntityInfo killer = null;
            EntityInfo user = EntityManager.GetEntityInfo(npc.KillerId);
            if (null != user) {
                killer = user;
            } else {
                EntityInfo parter = EntityManager.GetEntityInfo(npc.KillerId);
                if (null != parter && parter.OwnerId > 0) {
                    killer = EntityManager.GetEntityInfo(parter.OwnerId);
                }
            }
            if (null != killer) {
                killer.GetCombatStatisticInfo().AddKillNpcCount(1);
            }
        }
        private void CalcKillIncome(EntityInfo npc)
        {
            if (null == npc)
                return;
            UpdateKillCount(npc);
            //Death drop
            EntityInfo userKiller = EntityManager.GetEntityInfo(npc.KillerId);
        }
        
        public int GetBossCount()
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.EntityType == (int)EntityTypeEnum.Boss) {
                    ++ct;
                }
            }
            return ct;
        }
        public int GetBattleNpcCount()
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.IsCombatNpc()) {
                    ++ct;
                }
            }
            return ct;
        }
        public int GetBattleNpcCount(EntityInfo src, CharacterRelation relation)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.IsCombatNpc() && EntityInfo.GetRelation(src, info) == relation) {
                    ++ct;
                }
            }
            return ct;
        }
        public int GetBattleNpcCount(int campId, CharacterRelation relation)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.IsCombatNpc() && EntityInfo.GetRelation(campId, info.GetCampId()) == relation) {
                    ++ct;
                }
            }
            return ct;
        }
        public int GetBattleNpcCount(int campId)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.IsCombatNpc() && info.GetCampId() == campId) {
                    ++ct;
                }
            }
            return ct;
        }
        public int GetDyingBattleNpcCount(int campId, CharacterRelation relation)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && info.IsDead() && info.DeadTime != 0 && info.IsCombatNpc() && EntityInfo.GetRelation(campId, info.GetCampId()) == relation) {
                    ++ct;
                }
            }
            return ct;
        }
        public int GetDyingBattleNpcCount(int campId)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && info.IsDead() && info.DeadTime != 0 && info.IsCombatNpc() && info.GetCampId() == campId) {
                    ++ct;
                }
            }
            return ct;
        }
        public int GetNpcCount(int startUnitId, int endUnitId)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead()) {
                    int unitId = info.GetUnitId();
                    if (unitId >= startUnitId && unitId <= endUnitId) {
                        ++ct;
                    }
                }
            }
            return ct;
        }
        public void UpdateAirWallInfo(string wallName, bool state)
        {
            m_AirWallInfo[wallName] = state;
        }
    }
}
