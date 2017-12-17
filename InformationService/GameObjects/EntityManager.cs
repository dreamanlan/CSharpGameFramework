using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    public sealed class EntityManager
    {
        public DamageDelegation OnDamage;

        public EntityManager(int poolSize)
        {
            m_EntityPoolSize = poolSize;
        }

        public void SetSceneContext(SceneContextInfo context)
        {
            m_SceneContext = context;
        }

        public LinkedListDictionary<int, EntityInfo> Entities
        {
            get { return m_Entities; }
        }

        public EntityInfo GetEntityInfo(int id)
        {
            EntityInfo entity;
            m_Entities.TryGetValue(id, out entity);
            return entity;
        }

        public EntityInfo GetEntityInfoByUnitId(int id)
        {
            EntityInfo entity = null;
            for (LinkedListNode<EntityInfo> linkNode = m_Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo info = linkNode.Value;
                if (info.GetUnitId() == id) {
                    entity = info;
                    break;
                }
            }
            return entity;
        }

        public EntityInfo AddEntity(int unitId, int camp, TableConfig.Actor cfg, string ai, params string[] aiParams)
        {
            EntityInfo entity = NewEntityInfo();
            entity.SceneContext = m_SceneContext;
            entity.LoadData(unitId, camp, cfg, ai, aiParams);
            // born
            entity.IsBorning = true;
            entity.BornTime = 0;
            entity.SetAIEnable(false);
            m_Entities.AddLast(entity.GetId(), entity);
            return entity;
        }

        public EntityInfo AddEntity(int id, int unitId, int camp, TableConfig.Actor cfg, string ai, params string[] aiParams)
        {                
			if (m_Entities.Contains(id)) {
            	LogSystem.Warn("duplicate entity {0} !!!", id);
                return null;
            }
            EntityInfo entity = NewEntityInfo(id);
            entity.SceneContext = m_SceneContext;
            entity.LoadData(unitId, camp, cfg, ai, aiParams);
            entity.IsBorning = true;
            entity.BornTime = 0;
            entity.SetAIEnable(false);
            m_Entities.AddLast(entity.GetId(), entity);
            return entity;
        }

        public EntityInfo DelayAddEntity(int unitId, int camp, TableConfig.Actor cfg, string ai, params string[] aiParams)
        {
            EntityInfo entity = NewEntityInfo();
            entity.SceneContext = m_SceneContext;
            entity.LoadData(unitId, camp, cfg, ai, aiParams);
            entity.IsBorning = true;
            entity.BornTime = 0;
            entity.SetAIEnable(false);
            m_DelayAdd.Add(entity);
            return entity;
        }

        public EntityInfo DelayAddEntity(int id, int unitId, int camp, TableConfig.Actor cfg, string ai, params string[] aiParams)
        {
            EntityInfo entity = NewEntityInfo(id);
            entity.SceneContext = m_SceneContext;
            entity.LoadData(unitId, camp, cfg, ai, aiParams);
            entity.IsBorning = true;
            entity.BornTime = 0;
            entity.SetAIEnable(false);
            m_DelayAdd.Add(entity);
            return entity;
        }

        public void ExecuteDelayAdd()
        {
            for (int ix = 0; ix < m_DelayAdd.Count; ++ix) {
                var i = m_DelayAdd[ix];
                m_Entities[i.GetId()] = i;
            }
            m_DelayAdd.Clear();
        }

        public void RemoveEntity(int id)
        {
            EntityInfo entity = GetEntityInfo(id);
            if (null != entity) {
                m_Entities.Remove(id);
                entity.SceneContext = m_SceneContext;
                RecycleEntityInfo(entity);
            }
        }

        public EntityInfo GetNearest(ScriptRuntime.Vector3 pos, ref float minPowDist)
        {
            EntityInfo result = null;
            float powDist = 0.0f;
            for (LinkedListNode<EntityInfo> linkNode = m_Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo entity = linkNode.Value;
                if (null != entity && entity.IsCombatNpc()) {
                    powDist = Geometry.DistanceSquare(pos, entity.GetMovementStateInfo().GetPosition3D());
                    if (minPowDist > powDist) {
                        result = entity;
                        minPowDist = powDist;
                    }
                }
            }
            return result;
        }
        public bool HasCombatNpc()
        {
            bool result = false;
            for (LinkedListNode<EntityInfo> linkNode = m_Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo entity = linkNode.Value;
                if (null != entity && entity.IsCombatNpc()) {
                    result = true;
                    break;
                }
            }
            return result;
        }
        public bool HasCombatNpcAlive()
        {
            bool result = false;
            for (LinkedListNode<EntityInfo> linkNode = m_Entities.FirstNode; null != linkNode; linkNode = linkNode.Next) {
                EntityInfo entity = linkNode.Value;
                if (null != entity && entity.IsCombatNpc() && !entity.IsDead()) {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void Reset()
        {
            m_Entities.Clear();
            m_DelayAdd.Clear();
            m_UnusedEntities.Clear();
            m_NextInfoId = c_StartId;
            m_UnusedIds.Clear();
            m_UnusedClientIds.Clear();
        }

        public void FireDamageEvent(int receiver, int sender, bool isNormalDamage, bool isCritical, int hpDamage, int npDamage)
        {
            if (OnDamage != null) {
                OnDamage(receiver, sender, isNormalDamage, isCritical, hpDamage, npDamage);
            }
        }

        private EntityInfo NewEntityInfo()
        {
            EntityInfo entity = null;
            int id = GenNextId();
            if (m_UnusedEntities.Count > 0) {
                entity = m_UnusedEntities.Dequeue();
                entity.Reset();
                entity.InitId(id);
            } else {
                entity = new EntityInfo(id);
            }
            return entity;
        }

        private EntityInfo NewEntityInfo(int id)
        {
            EntityInfo entity = null;
            if (m_UnusedEntities.Count > 0) {
                entity = m_UnusedEntities.Dequeue();
                entity.Reset();
                entity.InitId(id);
            } else {
                entity = new EntityInfo(id);
            }
            return entity;
        }

        private void RecycleEntityInfo(EntityInfo npcInfo)
        {
            if (null != npcInfo) {
                int id = npcInfo.GetId();
                if (id >= c_StartId && id < c_StartId + c_MaxIdNum) {
                    m_UnusedIds.Push(id);
                }
                if (id >= c_StartId_Client && id < c_StartId_Client + c_MaxIdNum) {
                    m_UnusedClientIds.Push(id);
                }
                if (m_UnusedEntities.Count < m_EntityPoolSize) {
                    npcInfo.Reset();
                    m_UnusedEntities.Enqueue(npcInfo);
                }
            }
        }

        private int GenNextId()
        {
            int id = 0;
            int startId = 0;
            if (GlobalVariables.Instance.IsClient) {
                startId = c_StartId_Client;
                while (m_UnusedClientIds.Count > 100) {
                    int t = m_UnusedClientIds.Pop();
                    if (!m_Entities.Contains(t)) {
                        id = t;
                        break;
                    }
                }
            } else {
                startId = c_StartId;
                while (m_UnusedIds.Count > 100) {
                    int t = m_UnusedIds.Pop();
                    if (!m_Entities.Contains(t)) {
                        id = t;
                        break;
                    }
                }
            }
            if (id <= 0) {
                for (int i = 0; i < c_MaxIdNum; ++i) {
                    int t = (m_NextInfoId + i - startId) % c_MaxIdNum + startId;
                    if (!m_Entities.Contains(t)) {
                        id = t;
                        break;
                    }
                }
                if (id > 0) {
                    m_NextInfoId = (id + 1 - startId) % c_MaxIdNum + startId;
                }
            }
            return id;
        }

        private List<EntityInfo> m_DelayAdd = new List<EntityInfo>();
        private LinkedListDictionary<int, EntityInfo> m_Entities = new LinkedListDictionary<int, EntityInfo>();
        private Queue<EntityInfo> m_UnusedEntities = new Queue<EntityInfo>();
        private Heap<int> m_UnusedIds = new Heap<int>(new DefaultReverseComparer<int>());
        private Heap<int> m_UnusedClientIds = new Heap<int>(new DefaultReverseComparer<int>());
        private int m_EntityPoolSize = 128;

        private const int c_StartId = 100;
        private const int c_MaxIdNum = 1000;
        private const int c_StartId_Client = 2000;
        private int m_NextInfoId = c_StartId;

        private SceneContextInfo m_SceneContext = null;
    }
}
