﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptableFramework
{
    public sealed partial class Scene
    {
        public EntityInfo GetEntityById(int id)
        {
            EntityInfo obj = null;
            if (null != m_EntityMgr)
                obj = m_EntityMgr.GetEntityInfo(id);
            return obj;
        }
        public EntityInfo GetEntityByUnitId(int unitId)
        {
            EntityInfo obj = null;
            if (null != m_EntityMgr)
                obj = m_EntityMgr.GetEntityInfoByUnitId(unitId);
            return obj;
        }
        public void DestroyEntityById(int id)
        {
            if (m_EntityMgr.Entities.Contains(id)) {
                m_EntityMgr.RemoveEntity(id);
            }
        }
        public int CreateEntity(int unitId, float x, float y, float z, float dir, int camp, int tableId)
        {
            int objId = 0;
            TableConfig.Actor cfg = TableConfig.ActorProvider.Instance.GetActor(tableId);
            if (null != cfg) {
                EntityInfo entity = m_EntityMgr.AddEntity(unitId, camp, cfg, string.Empty);
                if (null != entity) {
                    entity.GetMovementStateInfo().SetPosition(x, y, z);
                    entity.GetMovementStateInfo().SetFaceDir(dir);
                    objId = entity.GetId();
                    OnCreateEntity(entity);
                }
            }
            return objId;
        }
        public int CreateEntity(int unitId, float x, float y, float z, float dir, int camp, int tableId, string ai, params string[] aiParams)
        {
            int objId = 0;
            TableConfig.Actor cfg = TableConfig.ActorProvider.Instance.GetActor(tableId);
            if (null != cfg) {
                EntityInfo entity = m_EntityMgr.AddEntity(unitId, camp, cfg, ai, aiParams);
                if (null != entity) {
                    entity.GetMovementStateInfo().SetPosition(x, y, z);
                    entity.GetMovementStateInfo().SetFaceDir(dir);
                    objId = entity.GetId();
                    OnCreateEntity(entity);
                }
            }
            return objId;
        }
        public int CreateSceneLogic(int configId, int logicId, params string[] args)
        {
            int id = 0;
            SceneLogicConfig cfg = new SceneLogicConfig();
            cfg.m_ConfigId = configId;
            cfg.m_LogicId = logicId;
            cfg.m_Params = args;
            SceneLogicInfo logicInfo = m_SceneLogicInfoMgr.AddSceneLogicInfo(cfg);
            if (null != logicInfo) {
                id = logicInfo.GetId();
            }
            return id;
        }
        public SceneLogicInfo CreateSceneLogic(int infoId, int configId, int logicId, params string[] args)
        {
            SceneLogicConfig cfg = new SceneLogicConfig();
            cfg.m_ConfigId = configId;
            cfg.m_LogicId = logicId;
            cfg.m_Params = args;
            SceneLogicInfo logicInfo = m_SceneLogicInfoMgr.AddSceneLogicInfo(infoId, cfg);
            return logicInfo;
        }
        public void DestroySceneLogic(int id)
        {
            m_SceneLogicInfoMgr.RemoveSceneLogicInfo(id);
        }
        public void DestroySceneLogicByConfigId(int configId)
        {
            SceneLogicInfo info = m_SceneLogicInfoMgr.GetSceneLogicInfoByConfigId(configId);
            if (null != info) {
                m_SceneLogicInfoMgr.RemoveSceneLogicInfo(info.GetId());
            }
        }
        public SceneLogicInfo GetSceneLogicInfo(int id)
        {
            SceneLogicInfo info = m_SceneLogicInfoMgr.GetSceneLogicInfo(id);
            return info;
        }
        public SceneLogicInfo GetSceneLogicInfoByConfigId(int configId)
        {
            SceneLogicInfo info = m_SceneLogicInfoMgr.GetSceneLogicInfoByConfigId(configId);
            return info;
        }

        private void DestroyEntity(EntityInfo ni)
        {
            ni.GetSkillStateInfo().RemoveAllImpact();
            OnDestroyEntity(ni);
            if (ni.IsCombatNpc()) {
                ni.DeadTime = 0;
            }
            DestroyEntityById(ni.GetId());
        }
        private void OnEntityKilled(EntityInfo ni)
        {
            int leftEnemyCt = GetBattleNpcCount((int)CampIdEnum.Blue, CharacterRelation.RELATION_ENEMY);
            int leftFriendCt = GetBattleNpcCount((int)CampIdEnum.Blue);

            m_StorySystem.SendMessage("obj_killed", ni.GetId(), leftEnemyCt, leftFriendCt);
            m_StorySystem.SendMessage("npc_killed:" + ni.GetUnitId(), ni.GetId(), leftEnemyCt, leftFriendCt);
        }
        private void TryAllKilledOrAllDied(bool tryAllKilled, bool tryAllDied)
        {
            if (tryAllKilled) {
                int leftEnemyCt = GetBattleNpcCount((int)CampIdEnum.Blue, CharacterRelation.RELATION_ENEMY) + GetDyingBattleNpcCount((int)CampIdEnum.Blue, CharacterRelation.RELATION_ENEMY);
                if (leftEnemyCt <= 0) {
                    m_StorySystem.SendMessage("all_killed");
                }
            }
            if (tryAllDied) {
                int leftFriendCt = GetBattleNpcCount((int)CampIdEnum.Blue) + GetDyingBattleNpcCount((int)CampIdEnum.Blue);
                if (leftFriendCt <= 0) {
                    m_StorySystem.SendMessage("all_died");
                }
            }
        }
    }
}
