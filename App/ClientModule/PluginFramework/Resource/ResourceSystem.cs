using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Reflection;

namespace GameFramework
{
    public enum PredefinedResourceGroup
    {
        Default = 0,
        PlayerSkillEffect,
        PlayerImpactEffect,
        PlayerBuffEffect,
        OtherSkillEffect,
        OtherImpactEffect,
        OtherBuffEffect,
        Miscellaneous,
        Sound,        
        MaxCount,        
    }
    /// <summary>
    /// Resource manager, which provides a resource cache reuse mechanism.
    /// 
    /// todo:After the split package strategy is determined, it needs to be modified to
    /// load resources from the subpackage.
    /// </summary>
    public class ResourceSystem
    {
        public delegate void ResourceLoadDelegation(UnityEngine.Object obj);
        public int MaxSameUnusedObjectNum
        {
            get { return m_MaxSameUnusedObjectNum; }
            set { m_MaxSameUnusedObjectNum = value; }
        }
        public int MaxUnusedObjectNum
        {
            get { return m_MaxUnusedObjectNum; }
            set { m_MaxUnusedObjectNum = value; }
        }
        public float MaxUnuseTimeForCleanup
        {
            get { return m_MaxUnuseTimeForCleanup; }
            set { m_MaxUnuseTimeForCleanup = value; }
        }
        public void InitGroup(int groupCount)
        {
            for (int i = m_GroupedResources.Count; i < groupCount; ++i) {
                m_GroupedResources.Add(new ResourceGroup());
            }
        }
        public void SetGroupMaxCount(int group, int maxCount)
        {
            int ct = m_GroupedResources.Count;
            if (group >= 0 && group < ct) {
                m_GroupedResources[group].MaxCount = maxCount;
            }
        }
        public int GetGroupResourceCount(int group)
        {
            int r = 0;
            if (group >= 0 && group < m_GroupedResources.Count) {
                r = m_GroupedResources[group].Resources.Count;
            }
            return r;
        }
        public void SetVisible(UnityEngine.GameObject obj, bool visible, CharacterController cc)
        {
            obj.SetActive(visible);
        }
        public void Init()
        {
            m_ResPoolRoot = new GameObject("ResPool");
            if (Application.isPlaying)
            {
                GameObject.DontDestroyOnLoad(m_ResPoolRoot);
            }
            m_ResPoolRootTransform = m_ResPoolRoot.transform;

            for (int i = 0; i < (int)PredefinedResourceGroup.MaxCount; ++i) {
                
                InitGroup(i);
            }
        }
        public void PreloadObject(string res, int ct)
        {
            PreloadSharedResource(res, false, (UnityEngine.Object prefab) => {
                for (int i = 0; i < ct; ++i) {
                    int resId = prefab.GetInstanceID();
                    UnityEngine.Object obj = GameObject.Instantiate(prefab);
                    if (null != obj) {
                        AddToLayerDict(obj);
                        FinalizeObject(obj);
                        AddToUnusedResources(resId, obj);
                    }
                }
            });
        }
        public void PreloadObject(string res)
        {
            PreloadSharedResource(res, false, (UnityEngine.Object prefab) => {
                int resId = prefab.GetInstanceID();
                UnityEngine.Object obj = GameObject.Instantiate(prefab);
                if (null != obj) {
                    AddToLayerDict(obj);
                    FinalizeObject(obj);
                    AddToUnusedResources(resId, obj);
                }
            });
        }
        public void PreloadSharedResource(string res)
        {
            PreloadSharedResource(res, false, null);
        }
        public bool CanNewObject(int group)
        {
            bool r = false;
            if (group >= 0 && group < m_GroupedResources.Count) {
                int ct = m_GroupedResources[group].Resources.Count;
                int maxCt = m_GroupedResources[group].MaxCount;
                r = ct < maxCt;
            }
            return r;
        }
        public UnityEngine.Object NewObject(string res)
        {
            return NewObject(res, 0.0f);
        }
        public UnityEngine.Object NewObject(string res, float timeToRecycle)
        {
            return NewObject(res, timeToRecycle, 0);
        }
        public UnityEngine.Object NewObject(string res, int group)
        {
            return NewObject(res, 0.0f, group);
        }
        public UnityEngine.Object NewObject(string res, float timeToRecycle, int group)
        {
            UnityEngine.Object prefab = GetSharedResource(res);
            return NewObject(prefab, timeToRecycle, group);
        }
        public UnityEngine.Object NewObject(UnityEngine.Object prefab)
        {
            return NewObject(prefab, 0.0f);
        }
        public UnityEngine.Object NewObject(UnityEngine.Object prefab, float timeToRecycle)
        {
            return NewObject(prefab, timeToRecycle, 0);
        }
        public UnityEngine.Object NewObject(UnityEngine.Object prefab, int group)
        {
            return NewObject(prefab, 0.0f, group);
        }
        public UnityEngine.Object NewObject(UnityEngine.Object prefab, float timeToRecycle, int group)
        {
            UnityEngine.Object obj = null;
            if (null != prefab && CanNewObject(group)) {
                float curTime = Time.time;
                float time = timeToRecycle;
                if (timeToRecycle > 0)
                    time += curTime;
                int resId = prefab.GetInstanceID();
                obj = NewFromUnusedResources(resId);
                if (null == obj) {
                    obj = GameObject.Instantiate(prefab);
                    AddToLayerDict(obj);
                }
                if (null != obj) {
                    AddToUsedResources(obj, resId, time, group);
                    InitializeObject(obj);
                }
            }
            return obj;
        }
        public bool RecycleObject(UnityEngine.Object obj)
        {
            bool ret = false;
            if (null != obj) {
                int objId = obj.GetInstanceID();
                UsedResourceInfo resInfo;
                if (m_UsedResources.TryGetValue(objId, out resInfo)) {
                    if (null != resInfo) {
                        FinalizeObject(resInfo.m_Object);
                        RemoveFromUsedResources(objId, resInfo.m_Group);
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
            for (LinkedListNode<UsedResourceInfo> node = m_UsedResources.FirstNode; null != node; ) {
                UsedResourceInfo resInfo = node.Value;
                if (resInfo.m_RecycleTime > 0 && resInfo.m_RecycleTime < curTime) {
                    node = node.Next;

                    UnityEngine.GameObject gameObject = resInfo.m_Object as UnityEngine.GameObject;
                    if (null != gameObject) {
                    }

                    FinalizeObject(resInfo.m_Object);
                    AddToUnusedResources(resInfo.m_ResId, resInfo.m_Object);
                    RemoveFromUsedResources(resInfo.m_ObjId, resInfo.m_Group);
                    resInfo.Recycle();
                } else {
                    node = node.Next;
                }
            }
            if (m_LastUnusedCheckTime + c_UnusedCheckInterval < curTime) {
                m_LastUnusedCheckTime = c_UnusedCheckInterval;

                bool fullCheck = m_ResPoolRootTransform.childCount >= m_MaxUnusedObjectNum;
                int ct = 0;
                foreach (var pair in m_UnusedResources) {
                    var heap = pair.Value;
                    if (heap.Count >= m_MaxSameUnusedObjectNum || fullCheck) {
                        try {
                            var tree = heap.LockData();
                            for (int i = tree.Count - 1; i >= 0; --i) {
                                var info = tree[i];
                                if (info.UnuseTime + m_MaxUnuseTimeForCleanup <= curTime) {
                                    ++ct;
                                    heap.SetDataDirty();
                                    tree.RemoveAt(i);
                                    GameObject go = info.Obj as GameObject;
                                    if (null != go) {
                                        go.transform.SetParent(null);
                                    }
                                    GameObject.Destroy(info.Obj);
                                    info.Recycle();
                                }
                            }
                        } finally {
                            heap.UnlockData();
                        }
                    }
                    if (ct > 10) {
                        break;
                    }
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
            for (LinkedListNode<UsedResourceInfo> node = m_UsedResources.FirstNode; null != node; ) {
                UsedResourceInfo resInfo = node.Value;
                node = node.Next;
                RemoveFromUsedResources(resInfo.m_ObjId, -1);
                resInfo.Recycle();
            }
            for (int i = 0; i < m_GroupedResources.Count; ++i) {
                m_GroupedResources[i].Resources.Clear();
            }

            foreach (var pair in m_UnusedResources) {
                int key = pair.Key;
                var heap = pair.Value;
                heap.Clear();
            }

            foreach (KeyValuePair<string, ObjectEx> pair in m_LoadedPrefabs) {
                string key = pair.Key;
                if (pair.Value != null && pair.Value.DontDestroyed)
                    continue;
                m_WaitDeleteLoadedPrefabEntrys.Add(key);
            }
            for (int i = 0; i < m_WaitDeleteLoadedPrefabEntrys.Count; i++) {
                m_LoadedPrefabs.Remove(m_WaitDeleteLoadedPrefabEntrys[i]);
            }
            m_WaitDeleteLoadedPrefabEntrys.Clear();

            Resources.UnloadUnusedAssets();
        }

        private UnityEngine.Object GetSharedResource(string res, bool notdestroyed)
        {
            UnityEngine.Object obj = null;
            if (string.IsNullOrEmpty(res)) {
                return obj;
            }
            ObjectEx objEx = null;
            if (!m_LoadedPrefabs.TryGetValue(res, out objEx) || null == objEx || null == objEx.Obj) {
                if (AssetBundleManager.Instance.Contains(res)) {
                    obj = AssetBundleManager.Instance.Load(res);
                }
                if (obj == null) {
                    obj = Resources.Load(res);
                }
                if (obj != null) {
                    objEx = new ObjectEx();
                    objEx.Obj = obj;
                    objEx.DontDestroyed = notdestroyed;
                    m_LoadedPrefabs[res] = objEx;
                } else {
                    UnityEngine.Debug.Log("LoadAsset failed:" + res);
                }
            } else if(objEx != null){
                obj = objEx.Obj;
            }
            return obj;
        }
        private void PreloadSharedResource(string res, bool notdestroyed, ResourceLoadDelegation callback)
        {
            if (string.IsNullOrEmpty(res)) {
                return;
            }
            if (!m_LoadedPrefabs.ContainsKey(res)) {
                m_LoadedPrefabs.Add(res, null);
                if (AssetBundleManager.Instance.Contains(res)) {
                    AssetBundleManager.Instance.LoadAsync(res, (UnityEngine.Object obj)=>{
                        PreloadFinishCallback(res, notdestroyed, obj);
                        if (null != callback) {
                            callback(obj);
                        }
                    });
                } else {
                    ResourceLoadDelegation callback0 = (UnityEngine.Object obj) => {
                        PreloadFinishCallback(res, notdestroyed, obj);
                        if (null != callback) {
                            callback(obj);
                        }
                    };
                    Utility.SendScriptMessage("LoadResourceAsync", new object[] { res, callback0 });
                }
            }
        }
        private void PreloadFinishCallback(string res, bool notdestroyed, UnityEngine.Object obj)
        {
            ObjectEx objEx;
            if (!m_LoadedPrefabs.TryGetValue(res, out objEx) || null == objEx || null == objEx.Obj) {
                if (obj != null) {
                    objEx = new ObjectEx();
                    objEx.Obj = obj;
                    objEx.DontDestroyed = notdestroyed;
                    m_LoadedPrefabs[res] = objEx;
                } else {
                    UnityEngine.Debug.Log("LoadAsset failed:" + res);
                }
            }
        }
        private UnityEngine.Object NewFromUnusedResources(int res)
        {
            UnityEngine.Object obj = null;
            Heap<UnusedObjectInfo> heap;
            if (m_UnusedResources.TryGetValue(res, out heap)) {
                if (heap.Count > 0) {
                    var info = heap.Pop();
                    obj = info.Obj;
                    info.Recycle();
                }
            }
            return obj;
        }
        private void AddToUnusedResources(int res, UnityEngine.Object obj)
        {
            Heap<UnusedObjectInfo> heap;
            if (!m_UnusedResources.TryGetValue(res, out heap)) {
                heap = new Heap<UnusedObjectInfo>(m_UnusedObjectInfoComparer);
                m_UnusedResources.Add(res, heap);
            }
            var info = m_UnusedObjectInfoPool.Alloc();
            info.Obj = obj;
            info.UnuseTime = Time.time;
            heap.Push(info);
        }
        private void AddToUsedResources(UnityEngine.Object obj, int resId, float recycleTime, int group)
        {
            int objId = obj.GetInstanceID();
            if (group >= 0 && group < m_GroupedResources.Count) {
                var grp = m_GroupedResources[group];
                if (!grp.Resources.Contains(objId)) {
                    grp.Resources.Add(objId);
                }
            }
            if (!m_UsedResources.Contains(objId)) {
                UsedResourceInfo info = m_UsedResourceInfoPool.Alloc();
                info.m_ObjId = objId;
                info.m_Object = obj;
                info.m_ResId = resId;
                info.m_RecycleTime = recycleTime;
                info.m_Group = group;

                m_UsedResources.AddLast(objId, info);
            }
        }
        private void AddToLayerDict(UnityEngine.Object obj)
        {
            GameObject gameObj = obj as GameObject;
            if (null != gameObj) {
                int instId = obj.GetInstanceID();
                if (!m_LayerDict.ContainsKey(instId)) {
                    m_LayerDict.Add(instId, gameObj.layer);
                }
            }
        }
        private void RemoveFromUsedResources(int objId, int group)
        {
            if (group >= 0 && group < m_GroupedResources.Count) {
                var grp = m_GroupedResources[group];
                grp.Resources.Remove(objId);
            }
            m_UsedResources.Remove(objId);
        }

        private void InitializeObject(UnityEngine.Object obj)
        {
            GameObject gameObj = obj as GameObject;
            if (null != gameObj) {
                gameObj.transform.SetParent(null);
                gameObj.SetActive(true);
                OnActiveChanged(gameObj, true);
            }
        }
        private void FinalizeObject(UnityEngine.Object obj)
        {
            GameObject gameObj = obj as GameObject;
            if (null != gameObj) {
                gameObj.transform.SetParent(m_ResPoolRootTransform, false);
                OnActiveChanged(gameObj, false);
                gameObj.SetActive(false);
            }
        }
        private void OnActiveChanged(UnityEngine.GameObject obj, bool active)
        {
            if (active) {
                int instId = obj.GetInstanceID();
                int layer;
                if (m_LayerDict.TryGetValue(instId, out layer)) {
                    SetLayer(obj, layer);
                }

                ParticleSystem[] pss = obj.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < pss.Length; i++) {
                    if (null != pss[i] && pss[i].main.playOnAwake) {
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
                ParticleSystem[] pss = obj.GetComponentsInChildren<ParticleSystem>(true);
                for (int i = 0; i < pss.Length; i++) {
                    if (null != pss[i] && pss[i].main.playOnAwake) {
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

        public static void ForceSetGameObjectLayer(GameObject obj, int layer)
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
            m_UnusedObjectInfoPool.Init(256);
        }

        private class UsedResourceInfo : IPoolAllocatedObject<UsedResourceInfo>
        {
            internal int m_ObjId;
            internal UnityEngine.Object m_Object;
            internal int m_ResId;
            internal float m_RecycleTime;
            internal int m_Group;

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

        private class ObjectEx
        {
            internal UnityEngine.Object Obj;
            internal bool DontDestroyed = false;
        }
        private class ResourceGroup
        {
            internal int MaxCount = int.MaxValue;
            internal HashSet<int> Resources = new HashSet<int>();
        }
        private List<ResourceGroup> m_GroupedResources = new List<ResourceGroup>();

        private MyDictionary<string, ObjectEx> m_LoadedPrefabs = new MyDictionary<string, ObjectEx>();
        private List<string> m_WaitDeleteLoadedPrefabEntrys = new List<string>();

        private LinkedListDictionary<int, UsedResourceInfo> m_UsedResources = new LinkedListDictionary<int, UsedResourceInfo>();
        private class UnusedObjectInfo : IPoolAllocatedObject<UnusedObjectInfo>
        {
            internal UnityEngine.Object Obj;
            internal float UnuseTime;
            internal void Recycle()
            {
                Obj = null;
                m_Pool.Recycle(this);
            }
            public void InitPool(ObjectPool<UnusedObjectInfo> pool)
            {
                m_Pool = pool;
            }
            public UnusedObjectInfo Downcast()
            {
                return this;
            }
            private ObjectPool<UnusedObjectInfo> m_Pool = null;
        }
        private class Comparer : IComparer<UnusedObjectInfo>
        {
            public int Compare(UnusedObjectInfo x, UnusedObjectInfo y)
            {
                if (x == null) {
                    return (y != null) ? -1 : 0;
                }
                if (y == null) {
                    return 1;
                }
                if (x.UnuseTime < y.UnuseTime)
                    return -1;
                else if (Geometry.IsSameFloat(x.UnuseTime, y.UnuseTime))
                    return 0;
                else
                    return 1;
            }
        }

        private int m_MaxSameUnusedObjectNum = 10;
        private float m_MaxUnuseTimeForCleanup = 5.0f;
        private int m_MaxUnusedObjectNum = 150;
        private ObjectPool<UnusedObjectInfo> m_UnusedObjectInfoPool = new ObjectPool<UnusedObjectInfo>();
        private Comparer m_UnusedObjectInfoComparer = new Comparer();
        private MyDictionary<int, Heap<UnusedObjectInfo>> m_UnusedResources = new MyDictionary<int, Heap<UnusedObjectInfo>>();
        private float m_LastUnusedCheckTime = 0;
        private const float c_UnusedCheckInterval = 10.0f;

        private MyDictionary<int, int> m_LayerDict = new MyDictionary<int, int>();
        private int m_DeactiveLayer = LayerMask.NameToLayer("DeActive");

        private GameObject m_ResPoolRoot = null;
        private Transform m_ResPoolRootTransform = null;
                
        public static ResourceSystem Instance
        {
            get { return s_Instance; }
        }
        private static ResourceSystem s_Instance = new ResourceSystem();
    }
}
