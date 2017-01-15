using System;
using System.Collections.Generic;
using GameFrameworkMessage;

namespace GameFramework
{
    public sealed partial class Scene
    {
        private void TickPreloading()
        {
            long curTime = TimeUtility.GetLocalMilliseconds();
            if (m_LastPreloadingTickTime + c_PreloadingTickInterval < curTime) {
                m_LastPreloadingTickTime = curTime;

                bool haveUser = false;
                bool canStart = true;
                foreach (User us in m_RoomInfo.RoomUsers) {
                    if (us.IsEntered) {
                        haveUser = true;
                    }
                }
                if (!haveUser) {
                    canStart = false;
                }

                if (canStart) {
                    LoadObjects();
                    RoomUserManager roomUserMgr = GetRoomUserManager();
                    if (null != roomUserMgr) {
                        foreach (User us in m_RoomInfo.RoomUsers) {
                            EnterScene(us);
                        }
                    }
                    //先让各客户端创建自己与场景相关信息
                    foreach (User us in m_RoomInfo.RoomUsers) {
                        if (us.IsEntered) {
                            //发玩家阵营给玩家
                            Msg_RC_CampChanged msg = new Msg_RC_CampChanged();
                            msg.obj_id = 0;
                            msg.camp_id = us.LobbyUserData.Camp;
                            us.SendMessage(RoomMessageDefine.Msg_RC_CampChanged, msg);
                            //将场景里的对象发给玩家
                            SyncSceneObjectsToUser(us);
                        }
                    }
                    //给观察者发初始玩家与场景对象信息
                    foreach (Observer observer in m_RoomInfo.RoomObservers) {
                        if (null != observer && !observer.IsIdle && observer.IsEntered) {
                            SyncForNewObserver(observer);
                        }
                    }
                    m_SceneState = SceneState.Running;
                }
            }
        }

        private void TickRunning()
        {
            TimeSnapshot.DoCheckPoint();

            m_KdTree.BeginBuild(m_EntityMgr.Entities.Count);
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                m_KdTree.AddObjForBuild(info);
            }
            m_KdTree.EndBuild();

            m_ServerDelayActionProcessor.HandleActions(100);
            m_SceneProfiler.DelayActionProcessorTime = TimeSnapshot.DoCheckPoint();

            m_MovementSystem.Tick();
            m_SceneProfiler.MovementSystemTime = TimeSnapshot.DoCheckPoint();

            TickAi();
            m_SceneProfiler.AiSystemTime = TimeSnapshot.DoCheckPoint();

            m_SceneLogicSystem.Tick();
            m_SceneProfiler.SceneLogicSystemTime = TimeSnapshot.DoCheckPoint();

            m_StorySystem.Tick();
            m_GmStorySystem.Tick();
            m_SceneProfiler.StorySystemTime = TimeSnapshot.DoCheckPoint();

            //技能逻辑Tick
            TickSkill();
            m_SceneProfiler.TickSkillTime = TimeSnapshot.DoCheckPoint();

            TickEntities();
            m_SceneProfiler.TickEntitiesTime = TimeSnapshot.DoCheckPoint();

            //属性同步
            if (0 == m_LastTickTimeForTickPerSecond) {
                m_LastTickTimeForTickPerSecond = TimeUtility.GetLocalMilliseconds();
                TickProperty();
            } else {
                long curTime = TimeUtility.GetLocalMilliseconds();
                if (curTime > m_LastTickTimeForTickPerSecond + c_IntervalPerSecond) {
                    m_LastTickTimeForTickPerSecond = curTime;
                    TickProperty();
                }
            }

            if (0 == m_LastTickTimeForTickPer5s) {
                m_LastTickTimeForTickPer5s = TimeUtility.GetLocalMilliseconds();
                ReloadObjects();
            } else {
                long curTime = TimeUtility.GetLocalMilliseconds();
                if (curTime > m_LastTickTimeForTickPer5s + c_IntervalPer5s) {
                    m_LastTickTimeForTickPer5s = curTime; 
                    ReloadObjects();
                }
            }

            m_SceneProfiler.TickAttrRecoverTime = TimeSnapshot.DoCheckPoint();
            //空间信息调试
            TickDebugSpaceInfo();
            m_SceneProfiler.TickDebugSpaceInfoTime = TimeSnapshot.DoCheckPoint();
        }
        
        private void TickEntities()
        {
            m_DeletedEntities.Clear();
            for (LinkedListNode<EntityInfo> linkNode = m_EntityMgr.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                info.RetireAttackerInfos(60000);
                if (info.LevelChanged || info.GetSkillStateInfo().BuffChanged) {
                    AttrCalculator.Calc(info);
                    info.LevelChanged = false;
                    info.GetSkillStateInfo().BuffChanged = false;
                }
                if (info.IsBorning) {
                    if (info.BornTime <= 0) {
                        SkillInfo skillInfo = info.GetSkillStateInfo().GetSkillInfoById(info.BornSkillId);
                        if (info.BornSkillId > 0 && null != skillInfo) {
                            info.BornTime = TimeUtility.GetLocalMilliseconds();
                            m_SkillSystem.StartSkill(info.GetId(), skillInfo.ConfigData, 0);
                        } else {
                            info.IsBorning = false;
                            info.BornTime = 0;
                            info.SetAIEnable(true);
                            info.RemoveState(CharacterPropertyEnum.x3009_无敌);
                        }
                    } else if (info.BornTime + info.BornTimeout < TimeUtility.GetLocalMilliseconds()) {
                        info.IsBorning = false;
                        info.BornTime = 0;
                        info.SetAIEnable(true);
                        info.RemoveState(CharacterPropertyEnum.x3009_无敌);
                    }
                }
                if (info.IsDead() && !info.NeedDelete) {
                    if (info.DeadTime <= 0) {
                        CalcKillIncome(info);
                        //发送npc死亡消息
                        Msg_RC_NpcDead npcDeadBuilder = new Msg_RC_NpcDead();
                        npcDeadBuilder.npc_id = info.GetId();
                        NotifyAllUser(RoomMessageDefine.Msg_RC_NpcDead, npcDeadBuilder);

                        SkillInfo skillInfo = info.GetSkillStateInfo().GetSkillInfoById(info.DeadSkillId);
                        if (info.DeadSkillId > 0 && null != skillInfo) {
                            info.DeadTime = TimeUtility.GetLocalMilliseconds();
                            m_SkillSystem.StopAllSkill(info.GetId(), true, false, true);
                            m_SkillSystem.StartSkill(info.GetId(), skillInfo.ConfigData, 0);
                            OnEntityKilled(info);
                        } else {
                            if (null == info.CustomData as User) {
                                info.DeadTime = 0;
                                info.NeedDelete = true;
                                OnEntityKilled(info);
                            } else {
                                info.DeadTime = TimeUtility.GetLocalMilliseconds();
                            }
                        }
                    } else {
                        if (null == info.CustomData as User && info.DeadTime + info.DeadTimeout < TimeUtility.GetLocalMilliseconds()) {
                            info.DeadTime = 0;
                            info.NeedDelete = true;

                            //重新发送npc死亡消息
                            Msg_RC_NpcDead npcDeadBuilder = new Msg_RC_NpcDead();
                            npcDeadBuilder.npc_id = info.GetId();
                            NotifyAllUser(RoomMessageDefine.Msg_RC_NpcDead, npcDeadBuilder);
                        } else if (null != info.CustomData as User && info.DeadTime + info.ReliveTimeout < TimeUtility.GetLocalMilliseconds()) {
                            info.DeadTime = 0;
                            info.Hp = info.HpMax;
                            info.Energy = info.EnergyMax;

                            Msg_RC_SyncProperty npcProp = DataSyncUtility.BuildSyncPropertyMessage(info);
                            NotifyAllUser(RoomMessageDefine.Msg_RC_SyncProperty, npcProp);
                        }
                    }
                }
                if (info.NeedDelete) {
                    m_DeletedEntities.Add(info);
                }
            }
            if (m_DeletedEntities.Count > 0) {
                int enemyCt = 0;
                int friendCt = 0;
                Msg_RC_DestroyNpc destroyNpcBuilder = new Msg_RC_DestroyNpc();
                for (int i = 0; i < m_DeletedEntities.Count; ++i) {
                    EntityInfo ni = m_DeletedEntities[i];
                    if (CharacterRelation.RELATION_ENEMY == EntityInfo.GetRelation((int)CampIdEnum.Blue, ni.GetCampId())) {
                        ++enemyCt;
                    } else if (CharacterRelation.RELATION_FRIEND == EntityInfo.GetRelation((int)CampIdEnum.Blue, ni.GetCampId())) {
                        ++friendCt;
                    }
                    //发送npc消失消息
                    destroyNpcBuilder.npc_id = ni.GetId();
                    NotifyAllUser(RoomMessageDefine.Msg_RC_DestroyNpc, destroyNpcBuilder);
                    DestroyEntity(ni);
                }
                TryAllKilledOrAllDied(enemyCt > 0, friendCt > 0);
            }
            m_EntityMgr.ExecuteDelayAdd();
        }

        private bool IsUserOnline(User user)
        {
            return null != user && user.UserControlState != (int)UserControlState.UserDropped && user.UserControlState != (int)UserControlState.Remove;
        }

        private void PublishRequirePveTimeEvent()
        {
            GameFrameworkMessage.Msg_RC_PublishEvent msg = new GameFrameworkMessage.Msg_RC_PublishEvent();
            msg.is_logic_event = false;
            msg.ev_name = "ge_require_pve_time";
            msg.group = "ui";
            NotifyAllUser(RoomMessageDefine.Msg_RC_PublishEvent, msg);
        }

        private void TickAi()
        {
            long curTime = TimeUtility.GetLocalMicroseconds();
            if (m_LastTickTimeForAi <= 0) {
                m_LastTickTimeForAi = curTime;
                return;
            }
            long deltaTime = curTime - m_LastTickTimeForAi;
            m_LastTickTimeForAi = curTime;

            for (int i = m_EntitiesForAi.Count - 1; i >= 0; --i) {
                var info = m_EntitiesForAi[i];
                if (info.GetAIEnable()) {
                    var aiStateInfo = info.GetAiStateInfo();
                    switch (aiStateInfo.CurState) {
                        case (int)PredefinedAiStateId.MoveCommand:
                            OnAiMoveCommand(info, deltaTime);
                            break;
                        case (int)PredefinedAiStateId.WaitCommand:
                            OnAiWaitCommand(info, deltaTime);
                            break;
                        case (int)PredefinedAiStateId.Idle:
                        default:
                            if (null != aiStateInfo.AiStoryInstanceInfo) {
                                var storyInstance = aiStateInfo.AiStoryInstanceInfo.m_StoryInstance;
                                if (null != storyInstance) {
                                    storyInstance.Tick(curTime);
                                }
                            }
                            break;
                    }
                }
            }
        }

        private void TickSkill()
        {
            m_SkillSystem.Tick();
        }

        private void TickDebugSpaceInfo()
        {
            if (GlobalVariables.Instance.IsDebug) {
                bool needDebug = false;
                foreach (User user in m_RoomInfo.RoomUsers) {
                    if (user.IsDebug) {
                        needDebug = true;
                        break;
                    }
                }
                if (needDebug) {
                    Msg_RC_DebugSpaceInfo builder = new Msg_RC_DebugSpaceInfo();
                    for (LinkedListNode<EntityInfo> linkNode = EntityManager.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                        EntityInfo info = linkNode.Value;
                        Msg_RC_DebugSpaceInfo.DebugSpaceInfo infoBuilder = new Msg_RC_DebugSpaceInfo.DebugSpaceInfo();
                        infoBuilder.obj_id = info.GetId();
                        infoBuilder.is_player = false;
                        infoBuilder.pos_x = (float)info.GetMovementStateInfo().GetPosition3D().X;
                        infoBuilder.pos_z = (float)info.GetMovementStateInfo().GetPosition3D().Z;
                        infoBuilder.face_dir = (float)info.GetMovementStateInfo().GetFaceDir();
                        builder.space_infos.Add(infoBuilder);
                    }
                    foreach (User user in m_RoomInfo.RoomUsers) {
                        if (user.IsDebug) {
                            user.SendMessage(RoomMessageDefine.Msg_RC_DebugSpaceInfo, builder);
                        }
                    }
                }
            }
        }

        private void TickProperty()
        {
            for (LinkedListNode<EntityInfo> linkNode = EntityManager.Entities.FirstValue; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (!info.IsDead() && info.PropertyChanged) {
                    info.PropertyChanged = false;

                    Msg_RC_SyncProperty builder = DataSyncUtility.BuildSyncPropertyMessage(info);
                    NotifyAllUser(RoomMessageDefine.Msg_RC_SyncProperty, builder);
                }
            }
        }
    }
}
