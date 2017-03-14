using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFrameworkMessage;
using ScriptRuntime;
using SkillSystem;

namespace GameFramework
{
    public class EntityController
    {
        public void Init(Scene scene, EntityManager mgr)
        {
            m_Scene = scene;
            m_EntityMgr = mgr;
        }

        public void Reset()
        {

        }

        public void Release()
        {
        }

        public void Tick()
        {
        }
        
        public bool ExistGameObject(int objId)
        {
            EntityInfo obj = m_Scene.EntityManager.GetEntityInfo(objId);
            return null != obj;
        }
        public EntityInfo GetGameObject(int objId)
        {
            EntityInfo obj = m_Scene.EntityManager.GetEntityInfo(objId);
            return obj;
        }
        public int GetEntityType(int objId)
        {
            int type = 0;
            EntityInfo obj = m_Scene.EntityManager.GetEntityInfo(objId);
            if (null != obj) {
                type = obj.EntityType;
            }
            return type;
        }
        public int GetEntityType(EntityInfo obj)
        {
            int type = 0;
            if (null != obj) {
                type = obj.EntityType;
            }
            return type;
        }
        public int GetCampId(int objId)
        {
            int campId = 0;
            EntityInfo entity = m_Scene.EntityManager.GetEntityInfo(objId);
            if (null != entity) {
                campId = entity.GetCampId();
            }
            return campId;
        }
        public int GetCampId(EntityInfo obj)
        {
            int campId = 0;
            if (null != obj) {
                campId = obj.GetCampId();
            }
            return campId;
        }
        public CharacterRelation GetRelation(int one, int other)
        {
            EntityInfo entity1 = m_Scene.EntityManager.GetEntityInfo(one);
            EntityInfo entity2 = m_Scene.EntityManager.GetEntityInfo(other);
            if (null == entity1 || null == entity2)
                return CharacterRelation.RELATION_INVALID;
            else
                return EntityInfo.GetRelation(entity1, entity2);
        }
        public CharacterRelation GetRelation(EntityInfo one, EntityInfo other)
        {
            if (null == one || null == other)
                return CharacterRelation.RELATION_INVALID;
            else
                return EntityInfo.GetRelation(one, other);
        }
        public bool HaveState(int objId, string state)
        {
            EntityInfo entity = m_Scene.EntityManager.GetEntityInfo(objId);
            if (null != entity) {
                return entity.HaveState((CharacterPropertyEnum)CharacterStateUtility.NameToState(state));
            }
            return false;
        }
        public void AddState(int objId, string state)
        {
            EntityInfo entity = m_Scene.EntityManager.GetEntityInfo(objId);
            if (null != entity) {
                entity.AddState((CharacterPropertyEnum)CharacterStateUtility.NameToState(state));
            }
        }
        public void RemoveState(int objId, string state)
        {
            EntityInfo entity = m_Scene.EntityManager.GetEntityInfo(objId);
            if (null != entity) {
                if (string.IsNullOrEmpty(state)) {
                    //entity.StateFlag = 0;
                } else {
                    entity.RemoveState((CharacterPropertyEnum)CharacterStateUtility.NameToState(state));
                }
            }
        }
        public void AddShield(int objId, TableConfig.Skill cfg, int seq)
        {
            EntityInfo entity = m_Scene.EntityManager.GetEntityInfo(objId);
            if (null != entity) {
                int v;
                if (cfg.attrValues.TryGetValue((int)CharacterPropertyEnum.x2012_护盾值, out v)) {
                    entity.Shield += v;
                }
            }
        }
        public void RemoveShield(int objId, TableConfig.Skill cfg, int seq)
        {
            EntityInfo entity = m_Scene.EntityManager.GetEntityInfo(objId);
            if (null != entity) {
                entity.Shield = 0;
            }
        }
        public void BornFinish(int objId)
        {
            EntityInfo entity = m_Scene.EntityManager.GetEntityInfo(objId);
            if (null != entity) {
                entity.IsBorning = false;
                entity.SetAIEnable(true);
                entity.RemoveState(CharacterPropertyEnum.x3009_无敌);
            }
        }
        public void DeadFinish(int objId)
        {
            EntityInfo entity = m_Scene.EntityManager.GetEntityInfo(objId);
            if (null != entity) {
                entity.DeadTime = 0;
                entity.NeedDelete = true;
            }
        }
        
        public EntityController()
        {
        }

        private Scene m_Scene = null;
        private EntityManager m_EntityMgr = null;
    }
}