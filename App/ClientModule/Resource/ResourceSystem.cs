using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace GameFramework
{
    /// <summary>
    /// 资源管理器，提供资源缓存重用机制。
    /// 
    /// todo:分包策略确定后需要修改为从分包里加载资源
    /// </summary>
    public class ResourceSystem
    {
        public void SetVisible(UnityEngine.GameObject obj, bool visible, CharacterController cc)
        {
            obj.SetActive(visible);
        }
        public void PreloadObject(string res, int ct)
        {
            for (int i = 0; i < ct; ++i) {
                PreloadObject(res);
            }
        }
        public void PreloadObject(string res)
        {
            UnityEngine.Object obj = null;
            UnityEngine.Object prefab = GetSharedResource(res);
            if (null != prefab) {
                int resId = prefab.GetInstanceID();
                obj = GameObject.Instantiate(prefab);
                if (null != obj) {
                    AddToLayerDict(obj);
                    FinalizeObject(obj);
                    AddToUnusedResources(resId, obj);
                }
            }
        }
        public void PreloadSharedResource(string res)
        {
            GetSharedResource(res);
        }
        public UnityEngine.Object NewObject(string res)
        {
            return NewObject(res, 0);
        }
        public UnityEngine.Object NewObject(string res, float timeToRecycle)
        {
            UnityEngine.Object prefab = GetSharedResource(res);
            return NewObject(prefab, timeToRecycle);
        }
        public UnityEngine.Object NewObject(UnityEngine.Object prefab)
        {
            return NewObject(prefab, 0);
        }
        public UnityEngine.Object NewObject(UnityEngine.Object prefab, float timeToRecycle)
        {
            UnityEngine.Object obj = null;
            if (null != prefab) {
                float curTime = Time.time;
                float time = timeToRecycle;
                if (timeToRecycle > 0)
                    time += curTime;
                int resId = prefab.GetInstanceID();
                obj = NewFromUnusedResources(resId);
                if (null == obj) {
                    obj = GameObject.Instantiate(prefab);
                }
                if (null != obj) {
                    AddToUsedResources(obj, resId, time);
                    AddToLayerDict(obj);

                    InitializeObject(obj);
                }
            }
            return obj;
        }
        public bool RecycleObject(UnityEngine.Object obj)
        {
            bool ret = false;
            if (null != obj) {
                UnityEngine.GameObject gameObject = obj as UnityEngine.GameObject;
                if (null != gameObject) {
                    //BridgeForGfxModule.BgLog("RecycleObject {0} {1}", gameObject.name, gameObject.tag);
                }

                int objId = obj.GetInstanceID();
                if (m_UsedResources.Contains(objId)) {
                    UsedResourceInfo resInfo = m_UsedResources[objId];
                    if (null != resInfo) {
                        FinalizeObject(resInfo.m_Object);
                        RemoveFromUsedResources(objId);
                        AddToUnusedResources(resInfo.m_ResId, obj);
                        resInfo.Recycle();
                        ret = true;
                    }
                }
            }
            return ret;
        }
        public void Tick()
        {
            float curTime = Time.time;
            /*
            if (m_LastTickTime <= 0) {
              m_LastTickTime = curTime;
              return;
            }
            float delta = curTime - m_LastTickTime;
            if (delta < 0.1f) {
              return;
            }
            m_LastTickTime = curTime;
            */

            for (LinkedListNode<UsedResourceInfo> node = m_UsedResources.FirstValue; null != node; ) {
                UsedResourceInfo resInfo = node.Value;
                if (resInfo.m_RecycleTime > 0 && resInfo.m_RecycleTime < curTime) {
                    node = node.Next;

                    UnityEngine.GameObject gameObject = resInfo.m_Object as UnityEngine.GameObject;
                    if (null != gameObject) {
                        //BridgeForGfxModule.BgLog("RecycleObject {0} {1} by Tick", gameObject.name, gameObject.tag);
                    }

                    FinalizeObject(resInfo.m_Object);
                    AddToUnusedResources(resInfo.m_ResId, resInfo.m_Object);
                    RemoveFromUsedResources(resInfo.m_ObjId);
                    resInfo.Recycle();
                } else {
                    node = node.Next;
                }
            }
        }
        public UnityEngine.Object GetSharedResource(string res)
        {
            bool notdestory = SharedResourcePath.Contains(res);
            notdestory = false;
            return GetSharedResource(res, notdestory);
        }

        public static HashSet<string> SharedResourcePath = new HashSet<string>() {
        };

        public void CleanupResourcePool()
        {
            for (LinkedListNode<UsedResourceInfo> node = m_UsedResources.FirstValue; null != node; ) {
                UsedResourceInfo resInfo = node.Value;
                node = node.Next;
                RemoveFromUsedResources(resInfo.m_ObjId);
                resInfo.Recycle();
            }

            foreach (KeyValuePair<int, Queue<UnityEngine.Object>> pair in m_UnusedResources) {
                int key = pair.Key;
                Queue<UnityEngine.Object> queue = pair.Value;
                queue.Clear();
            }

            foreach (KeyValuePair<string, OjbectEx> pair in m_LoadedPrefabs) {
                string key = pair.Key;
                if (pair.Value.notdestroyed)
                    continue;
                m_WaitDeleteLoadedPrefabEntrys.Add(key);
            }
            for (int i = 0; i < m_WaitDeleteLoadedPrefabEntrys.Count; i++) {
                m_LoadedPrefabs.Remove(m_WaitDeleteLoadedPrefabEntrys[i]);
            }
            m_WaitDeleteLoadedPrefabEntrys.Clear();

            Resources.UnloadUnusedAssets();
        }

        private UnityEngine.Object GetSharedResource(string res, bool notdestoryed)
        {
            UnityEngine.Object obj = null;
            if (string.IsNullOrEmpty(res)) {
                return obj;
            }
            OjbectEx objEx = null;
            if (!m_LoadedPrefabs.TryGetValue(res, out objEx)) {
                if (AssetBundleManager.Instance.Contains(res)) {
                    obj = AssetBundleManager.Instance.Load(res);
                }
                if (obj == null) {
                    obj = Resources.Load(res);
                }
                if (obj != null) {
                    objEx = new OjbectEx();
                    objEx.obj = obj;
                    objEx.notdestroyed = notdestoryed;
                    m_LoadedPrefabs.Add(res, objEx);
                } else {
                    UnityEngine.Debug.Log("LoadAsset failed:" + res);
                }
            } else {
                obj = objEx.obj;
            }
            return obj;
        }
        private UnityEngine.Object NewFromUnusedResources(int res)
        {
            UnityEngine.Object obj = null;
            Queue<UnityEngine.Object> queue;
            if (m_UnusedResources.TryGetValue(res, out queue)) {
                if (queue.Count > 0)
                    obj = queue.Dequeue();
            }
            return obj;
        }
        private void AddToUnusedResources(int res, UnityEngine.Object obj)
        {
            Queue<UnityEngine.Object> queue;
            if (m_UnusedResources.TryGetValue(res, out queue)) {
                queue.Enqueue(obj);
            } else {
                queue = new Queue<UnityEngine.Object>();
                queue.Enqueue(obj);
                m_UnusedResources.Add(res, queue);
            }
        }
        private void AddToUsedResources(UnityEngine.Object obj, int resId, float recycleTime)
        {
            int objId = obj.GetInstanceID();
            if (!m_UsedResources.Contains(objId)) {
                UsedResourceInfo info = m_UsedResourceInfoPool.Alloc();
                info.m_ObjId = objId;
                info.m_Object = obj;
                info.m_ResId = resId;
                info.m_RecycleTime = recycleTime;

                m_UsedResources.AddLast(objId, info);
            }
        }
        private void AddToLayerDict(UnityEngine.Object obj)
        {
            GameObject gameObj = obj as GameObject;
            if (null != gameObj) {
                if (!m_LayerDict.ContainsKey(obj)) {
                    m_LayerDict.Add(obj, gameObj.layer);
                }
            }
        }
        private void RemoveFromUsedResources(int objId)
        {
            m_UsedResources.Remove(objId);
        }

        private void InitializeObject(UnityEngine.Object obj)
        {
            GameObject gameObj = obj as GameObject;
            if (null != gameObj) {
                OnActiveChanged(gameObj, true);
                gameObj.SetActive(true);
            }
        }
        private void FinalizeObject(UnityEngine.Object obj)
        {
            GameObject gameObj = obj as GameObject;
            if (null != gameObj) {
                if (null != gameObj.transform.parent)
                    gameObj.transform.parent = null;
                OnActiveChanged(gameObj, false);
                gameObj.SetActive(false);
            }
        }
        private void OnActiveChanged(UnityEngine.GameObject obj, bool active)
        {
            if (active) {
                /*
                if (!obj.activeSelf)
                    obj.SetActive(active);                
                int layer;
                if (m_LayerDict.TryGetValue(obj, out layer)) {
                    SetLayer(obj, layer);
                }
                */
                ParticleSystem[] pss = obj.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < pss.Length; i++) {
                    if (null != pss[i] && pss[i].playOnAwake) {
                        //pss[i].Clear(true);
                        pss[i].Play(true);
                    }
                }
                AudioSource[] audioSources = obj.GetComponentsInChildren<AudioSource>(true);
                for (int i = 0; i < audioSources.Length; i++) {
                    if (null != audioSources[i] && audioSources[i].playOnAwake) {
                        audioSources[i].Play();
                    }
                }
                NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
                if (null != agent) {
                    agent.enabled = false;
                }
                AbstractScriptBehavior[] scriptBehaviors = obj.GetComponentsInChildren<AbstractScriptBehavior>(true);
                for (int i = 0; i < scriptBehaviors.Length; ++i) {
                    if (null != scriptBehaviors[i]) {
                        scriptBehaviors[i].ResourceEnabled = true;
                    }
                }
            } else {
                /*
                if (obj.layer != m_DeactiveLayer)
                    m_LayerDict[obj] = obj.layer;
                SetLayer(obj, m_DeactiveLayer);
                */
                ParticleSystem[] pss = obj.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < pss.Length; i++) {
                    if (null != pss[i] && pss[i].playOnAwake) {
                        pss[i].Clear(true);
                        pss[i].Stop(true);
                    }
                }
                AudioSource[] audioSources = obj.GetComponentsInChildren<AudioSource>(true);
                for (int i = 0; i < audioSources.Length; i++) {
                    if (null != audioSources[i] && audioSources[i].playOnAwake) {
                        audioSources[i].Stop();
                    }
                }

                NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
                if (null != agent) {
                    agent.enabled = false;
                }
                AbstractScriptBehavior[] scriptBehaviors = obj.GetComponentsInChildren<AbstractScriptBehavior>(true);
                for (int i = 0; i < scriptBehaviors.Length; ++i) {
                    if (null != scriptBehaviors[i]) {
                        scriptBehaviors[i].ResourceEnabled = false;
                    }
                }
            }
        }

        public static void FroceSetGameObjectLayer(GameObject obj, int layer)
        {
            SetLayer(obj, layer);
        }
        private static void SetLayer(GameObject obj, int layer)
        {
            Transform t = obj.transform;
            if (null != t) {
                SetLayer(t, layer);
            }
        }
        private static void SetLayer(Transform t, int layer)
        {
            t.gameObject.layer = layer;
            for (int i = 0; i < t.childCount; ++i) {
                Transform c = t.GetChild(i);
                if (null != c) {
                    SetLayer(c, layer);
                }
            }
        }

        private ResourceSystem()
        {
            m_UsedResourceInfoPool.Init(256);
        }

        private class UsedResourceInfo : IPoolAllocatedObject<UsedResourceInfo>
        {
            internal int m_ObjId;
            internal UnityEngine.Object m_Object;
            internal int m_ResId;
            internal float m_RecycleTime;

            internal void Recycle()
            {
                m_Object = null;
                m_Pool.Recycle(this);
            }
            public void InitPool(ObjectPool<UsedResourceInfo> pool)
            {
                m_Pool = pool;
            }
            public UsedResourceInfo Downcast()
            {
                return this;
            }
            private ObjectPool<UsedResourceInfo> m_Pool = null;
        }

        private ObjectPool<UsedResourceInfo> m_UsedResourceInfoPool = new ObjectPool<UsedResourceInfo>();

        public class OjbectEx
        {
            public UnityEngine.Object obj;
            public bool notdestroyed = false;
        }
        private Dictionary<string, OjbectEx> m_LoadedPrefabs = new Dictionary<string, OjbectEx>();
        private List<string> m_WaitDeleteLoadedPrefabEntrys = new List<string>();

        private LinkedListDictionary<int, UsedResourceInfo> m_UsedResources = new LinkedListDictionary<int, UsedResourceInfo>();
        private Dictionary<int, Queue<UnityEngine.Object>> m_UnusedResources = new Dictionary<int, Queue<UnityEngine.Object>>();

        private Dictionary<UnityEngine.Object, int> m_LayerDict = new Dictionary<UnityEngine.Object, int>();
        //private float m_LastTickTime = 0;
        private int m_DeactiveLayer = LayerMask.NameToLayer("DeActive");

        public static ResourceSystem Instance
        {
            get { return s_Instance; }
        }
        private static ResourceSystem s_Instance = new ResourceSystem();
    }
}
