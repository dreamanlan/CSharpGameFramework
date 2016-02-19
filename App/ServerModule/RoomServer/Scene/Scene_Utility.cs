using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFrameworkMessage;
using ScriptRuntime;

namespace GameFramework
{
    internal sealed partial class Scene
    {
        private void LoadObjects()
        {
            LogSys.Log(LOG_TYPE.DEBUG, "Scene {0} start Running.", m_SceneResId);
            m_GameTime.Start();
            m_SceneContext.StartTime = m_GameTime.StartTime;

            ServerStorySystem.ThreadInitMask();
            m_StorySystem.ClearStoryInstancePool();
            m_StorySystem.PreloadSceneStories();
            m_StorySystem.StartStory("local_main");
            m_StorySystem.StartStory("story_main");
        }
        
        private void SyncSceneObjectsToUser(User user)
        {
            if (null != user) {
                EntityInfo userInfo = user.Info;
                Room room = GetRoom();
                if (null != userInfo && null != room && null != room.GetActiveScene()) {
                    for (LinkedListNode<EntityInfo> linkNode = EntityManager.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                        EntityInfo npc = linkNode.Value;
                        if (null != npc) {
                            Msg_RC_CreateNpc bder = DataSyncUtility.BuildCreateNpcMessage(npc);
                            user.SendMessage(RoomMessageDefine.Msg_RC_CreateNpc, bder);
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
                Room room = GetRoom();
                if (null != userInfo && null != room && null != room.GetActiveScene()) {
                    for (LinkedListNode<EntityInfo> linkNode = EntityManager.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                        EntityInfo npc = linkNode.Value;
                        if (null != npc && npc != userInfo) {
                            User other = npc.CustomData as User;
                            if (null != other) {
                                other.SendMessage(RoomMessageDefine.Msg_RC_CreateNpc, bder);
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
            //死亡掉落
            EntityInfo userKiller = EntityManager.GetEntityInfo(npc.KillerId);
        }
        
        internal int GetBossCount()
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.EntityType == (int)EntityTypeEnum.Boss) {
                    ++ct;
                }
            }
            return ct;
        }
        internal int GetBattleNpcCount()
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.IsCombatNpc()) {
                    ++ct;
                }
            }
            return ct;
        }
        internal int GetBattleNpcCount(EntityInfo src, CharacterRelation relation)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.IsCombatNpc() && EntityInfo.GetRelation(src, info) == relation) {
                    ++ct;
                }
            }
            return ct;
        }
        internal int GetBattleNpcCount(int campId, CharacterRelation relation)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.IsCombatNpc() && EntityInfo.GetRelation(campId, info.GetCampId()) == relation) {
                    ++ct;
                }
            }
            return ct;
        }
        internal int GetBattleNpcCount(int campId)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && !info.IsDead() && info.IsCombatNpc() && info.GetCampId() == campId) {
                    ++ct;
                }
            }
            return ct;
        }
        internal int GetDyingBattleNpcCount(int campId, CharacterRelation relation)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && info.IsDead() && info.DeadTime != 0 && info.IsCombatNpc() && EntityInfo.GetRelation(campId, info.GetCampId()) == relation) {
                    ++ct;
                }
            }
            return ct;
        }
        internal int GetDyingBattleNpcCount(int campId)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (null != info && info.IsDead() && info.DeadTime != 0 && info.IsCombatNpc() && info.GetCampId() == campId) {
                    ++ct;
                }
            }
            return ct;
        }
        internal int GetNpcCount(int startUnitId, int endUnitId)
        {
            int ct = 0;
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
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
        internal void UpdateAirWallInfo(string wallName, bool state)
        {
            m_AirWallInfo[wallName] = state;
        }
    }
}
