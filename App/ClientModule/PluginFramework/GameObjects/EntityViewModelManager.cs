using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework.Skill;

namespace GameFramework
{
    public class EntityViewModelManager
    {
        public void Init()
        {
        }

        public void Release()
        {
        }

        public void Tick()
        {
            foreach (KeyValuePair<int, EntityViewModel> pair in m_EntityViews) {
                EntityViewModel view = pair.Value;
                view.Update();
            }
            if (m_SpaceInfoViews.Count > 0) {
                if (!GlobalVariables.Instance.IsDebug) {
                    MarkSpaceInfoViews();
                    DestroyUnusedSpaceInfoViews();
                }
            }
        }

        public void CreateEntityView(int objId)
        {
            if (objId <= 0) {
                LogSystem.Error("Error:CreateEntityView objId<=0 !!!");
                Helper.LogCallStack();
                return;
            }
            if (!m_EntityViews.ContainsKey(objId)) {
                EntityInfo obj = PluginFramework.Instance.EntityManager.GetEntityInfo(objId);
                if (null != obj) {
                    EntityViewModel view = new EntityViewModel();
                    view.Create(obj);
                    m_EntityViews.Add(objId, view);
                    if (null != view.Actor) {
                        if (m_Object2Ids.ContainsKey(view.Actor)) {
                            m_Object2Ids[view.Actor] = objId;
                        } else {
                            m_Object2Ids.Add(view.Actor, objId);
                        }
                    } else {
                        LogSystem.Warn("CreateEntityView:{0}, model:{1}, actor is null", objId, obj.GetModel());
                    }
                }
            }
        }
        public void DestroyEntityView(int objId)
        {
            EntityViewModel view;
            if (m_EntityViews.TryGetValue(objId, out view)) {
                if (view != null && null != view.Actor) {
                    m_Object2Ids.Remove(view.Actor);
                    view.Destroy();
                }
                m_EntityViews.Remove(objId);
            }
        }

        public EntityViewModel GetEntityViewById(int objId)
        {
            EntityViewModel view = null;
            m_EntityViews.TryGetValue(objId, out view);
            return view;
        }
        public EntityViewModel GetEntityViewByUnitId(int unitId)
        {
            EntityViewModel view = null;
            EntityInfo obj = PluginFramework.Instance.GetEntityByUnitId(unitId);
            if (null != obj) {
                m_EntityViews.TryGetValue(obj.GetId(), out view);
            }
            return view;
        }        
        public UnityEngine.GameObject GetGameObject(int objId)
        {
            UnityEngine.GameObject obj = null;
            EntityViewModel view = GetEntityViewById(objId);
            if (null != view) {
                obj = view.Actor;
            }
            return obj;
        }
        public UnityEngine.GameObject GetGameObjectByUnitId(int unitId)
        {
            UnityEngine.GameObject obj = null;
            EntityViewModel view = GetEntityViewByUnitId(unitId);
            if (null != view) {
                obj = view.Actor;
            }
            return obj;
        }
        public EntityViewModel GetEntityView(UnityEngine.GameObject obj)
        {
            EntityViewModel view = null;
            int id;
            if (m_Object2Ids.TryGetValue(obj, out id)) {
                m_EntityViews.TryGetValue(id, out view);
            }
            return view;
        }
        public int GetGameObjectUnitId(UnityEngine.GameObject obj)
        {
            int unitId = 0;
            EntityViewModel view = GetEntityView(obj);
            if (null != view) {
                unitId = view.Entity.GetUnitId();
            }
            return unitId;
        }
        public int GetGameObjectId(UnityEngine.GameObject obj)
        {
            int id;
            m_Object2Ids.TryGetValue(obj, out id);
            return id;
        }
        public bool ExistGameObject(UnityEngine.GameObject obj)
        {
            return m_Object2Ids.ContainsKey(obj);
        }
        public bool ExistGameObject(int objId)
        {
            return m_EntityViews.ContainsKey(objId);
        }
        
        public void MarkSpaceInfoViews()
        {
            foreach (KeyValuePair<int, SpaceInfoView> pair in m_SpaceInfoViews) {
                SpaceInfoView view = pair.Value;
                view.NeedDestroy = true;
            }
        }
        public void UpdateSpaceInfoView(int objId, bool isPlayer, float x, float y, float z, float dir)
        {
            SpaceInfoView view = GetSpaceInfoViewById(objId);
            if (null == view) {
                view = CreateSpaceInfoView(objId, isPlayer);
            }
            if (null != view) {
                view.NeedDestroy = false;
                view.Update(x, y, z, dir);
            }
        }
        public void DestroyUnusedSpaceInfoViews()
        {
            List<int> deletes = new List<int>();
            foreach (KeyValuePair<int, SpaceInfoView> pair in m_SpaceInfoViews) {
                SpaceInfoView view = pair.Value;
                if (view.NeedDestroy)
                    deletes.Add(view.ObjId);
            }
            foreach (int id in deletes) {
                DestroySpaceInfoView(id);
            }
            deletes.Clear();
        }

        private SpaceInfoView CreateSpaceInfoView(int objId, bool isPlayer)
        {
            SpaceInfoView view = null;
            if (!m_SpaceInfoViews.ContainsKey(objId)) {
                view = new SpaceInfoView();
                view.Create(objId, isPlayer);
                m_SpaceInfoViews.Add(objId, view);
            }
            return view;
        }
        private void DestroySpaceInfoView(int objId)
        {
            SpaceInfoView view;
            if (m_SpaceInfoViews.TryGetValue(objId, out view)) {
                if (view != null) {
                    view.Destroy();
                }
                m_SpaceInfoViews.Remove(objId);
            }
        }
        private SpaceInfoView GetSpaceInfoViewById(int objId)
        {
            SpaceInfoView view = null;
            m_SpaceInfoViews.TryGetValue(objId, out view);
            return view;
        }

        private EntityViewModelManager()
        {
        }

        private MyDictionary<int, EntityViewModel> m_EntityViews = new MyDictionary<int, EntityViewModel>();
        private MyDictionary<UnityEngine.GameObject, int> m_Object2Ids = new MyDictionary<UnityEngine.GameObject, int>();

        private MyDictionary<int, SpaceInfoView> m_SpaceInfoViews = new MyDictionary<int, SpaceInfoView>();

        public static EntityViewModelManager Instance
        {
            get { return s_instance_; }
        }

        private static EntityViewModelManager s_instance_ = new EntityViewModelManager();
    }
}
