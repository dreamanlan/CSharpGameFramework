using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ProjectAnalyzer
{
    class RuntimeParticleanalyzer : EditorWindow
    {
        private List<ParticleSystem> mParticleSystems = new List<ParticleSystem>();
        private List<RuntimeParticleInfo> mParticleInfos = new List<RuntimeParticleInfo>();
        private GameObject[] mGoes;
        private int mMaxParticleCnt;
        private int ParticleSystemCnt = 0;


        float timeCollectGameObject = 0;
        float timeAnalyzerParticle = 0;

        private int curPage = 0;
        private int totalpageCnt;
        private int pageCnt = 20;
        private int startCnt = 0;
        private int endCnt = 0;
        private int showCnt = 0;
        private bool mLogoutParticleSystemes = true;
        private SortType sortType = SortType.ParticleCount;

        int orgStartY = 100;
        int startY = 0;

        public class RuntimeParticleInfo
        {
            public String name;
            public GameObject go;
            public float duration;
            public int maxParticles;
            public int particleCount ;
        }

        
        [MenuItem("ProjectAnalyzer/RuntimeParticleanalyzer")]
        static void Particleanalyzer()
        {
            RuntimeParticleanalyzer window = (RuntimeParticleanalyzer)EditorWindow.GetWindow(typeof(RuntimeParticleanalyzer));
            window.title = "RuntimeParticleanalyzer";
            window.minSize = new UnityEngine.Vector2(1000, 500);
        }

        void Update()
        {
            timeCollectGameObject += Time.deltaTime;           
            if (timeCollectGameObject > 5)
            {
                timeCollectGameObject = 0;
                collectGo();              
            }

            timeAnalyzerParticle += Time.deltaTime;
            if (timeAnalyzerParticle > 1)
            {
                timeAnalyzerParticle = 0;
                AnalyzerParticle();             
            }
        }
        void OnGUI()
        {
            try
            {
                showCnt = 0;
                startY = orgStartY;
                mMaxParticleCnt = 0;
                ParticleSystemCnt = 0;

                for (int i = 0; i < mParticleInfos.Count; i++)
                {
                    RuntimeParticleInfo info = mParticleInfos[i];
                    ParticleSystemCnt++;
                    mMaxParticleCnt += info.particleCount;
                }
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button(LanguageCfg.REFRESH, GUILayout.Width(100)))
                {
                    collectGo();
                    AnalyzerParticle();
                }

                totalpageCnt = ParticleSystemCnt % pageCnt > 0 ? ParticleSystemCnt / pageCnt + 1 : ParticleSystemCnt / pageCnt;
                if ( GUILayout.Button("Sys " + ParticleSystemCnt + "  ParticleCnt " + mMaxParticleCnt, GUILayout.Width(200)))
                {
                     mLogoutParticleSystemes = !mLogoutParticleSystemes;
                }

                GUILayout.Button((curPage + 1) + "/" + totalpageCnt, GUILayout.Width(100));

                if (GUILayout.Button("上一页", GUILayout.Width(100)))
                {
                    LastPage();
                }

                if (GUILayout.Button("下一页", GUILayout.Width(100)))
                {
                    NextPage();
                }              
                GUILayout.EndHorizontal();


                //绘制title
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(LanguageCfg.NAME, GUILayout.Width(150)))
                {
                    sortType = SortType.Name;
                    SortParticle(SortType.Name);
                }
                if (GUILayout.Button(LanguageCfg.PARTICLE_SYS_MAX_DURATION, GUILayout.Width(100)))
                {
                    sortType = SortType.Duration;
                    SortParticle(SortType.Duration);
                }
                if (GUILayout.Button(LanguageCfg.PARTICLE_SYS_MAX_PARTICLES, GUILayout.Width(100)))
                {
                    sortType = SortType.MaxParticles;
                    SortParticle(SortType.MaxParticles);
                }
                if (GUILayout.Button(LanguageCfg.PARTICLE_SYS_CURRENT_CNT, GUILayout.Width(100)))
                {
                    sortType = SortType.ParticleCount;
                    SortParticle(SortType.ParticleCount);
                }
                GUILayout.EndHorizontal();

                if (mLogoutParticleSystemes)
                {



                    GUILayout.BeginVertical();
                    for (int i = 0; i < mParticleInfos.Count; i++)
                    {
                        
                      
                        startCnt = curPage * pageCnt;
                        endCnt = (curPage + 1) * pageCnt;
                        endCnt = endCnt >= ParticleSystemCnt ? ParticleSystemCnt : endCnt;

                        
                        if (i < startCnt)
                        {
                            continue;
                        }
                        if (i >= endCnt)
                        {
                            return;
                        }


                        RuntimeParticleInfo info = mParticleInfos[i];

                        GUILayout.BeginHorizontal();
                        if (GUILayout.Button(info.name, GUILayout.Width(150)))
                        {
                            Selection.activeObject = info.go;
                        }

                        GUILayout.Label(info.duration.ToString(), GUILayout.Width(100));
                        GUILayout.Label(info.maxParticles.ToString(), GUILayout.Width(100));                     
                        GUILayout.Label(info.particleCount + "", GUILayout.Width(100));
                        GUILayout.EndHorizontal();
                       
                        startY += 45;
                        showCnt++;
                      
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();

            }
            catch (Exception e)
            {
                collectGo();
                return;
            }
        }

        public enum SortType
        {
            Name,
            Duration,
            MaxParticles,
            ParticleCount
        }

        void SortParticle(SortType sortType)
        {
            switch (sortType)
            {
                case SortType.Name:
                    mParticleInfos.Sort(delegate(RuntimeParticleInfo tInfo1, RuntimeParticleInfo tInfo2) { return tInfo1.name.CompareTo(tInfo2.name); });
                    break;
                case SortType.Duration:
                    mParticleInfos.Sort(delegate(RuntimeParticleInfo tInfo1, RuntimeParticleInfo tInfo2) { return tInfo2.duration.CompareTo(tInfo1.duration); });
                    break;
                case SortType.MaxParticles:
                    mParticleInfos.Sort(delegate(RuntimeParticleInfo tInfo1, RuntimeParticleInfo tInfo2) { return tInfo2.maxParticles.CompareTo(tInfo1.maxParticles); });
                    break;
                case SortType.ParticleCount:
                    mParticleInfos.Sort(delegate(RuntimeParticleInfo tInfo1, RuntimeParticleInfo tInfo2) { return tInfo2.particleCount.CompareTo(tInfo1.particleCount); });
                    break;
              
            }
        }

        void collectGo()
        {
            mParticleSystems.Clear();
            mGoes = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
            for (int i = 0; i < mGoes.Length; i++)
            {
                GameObject go = mGoes[i];
                if (go.GetComponent<ParticleRendererGetEnable>() == null)
                    go.AddComponent<ParticleRendererGetEnable>();
                ParticleSystem[] pses = go.GetComponentsInChildren<ParticleSystem>();
                for (int j = 0; j < pses.Length; j++)
                {
                    mParticleSystems.Add(pses[j]);
                }
            }
        }



        private void AnalyzerParticle()
        {
            try
            {
                mParticleInfos.Clear();
                for (int i = 0; i < mParticleSystems.Count; i++)
                {
                   
                    ParticleSystem ps = mParticleSystems[i];

                    if (ps.gameObject.activeSelf == false)
                    {
                        continue;
                    }
                    if (ps.gameObject.GetComponent<ParticleRendererGetEnable>().mEnabled == false)
                    {
                        continue;
                    }
                    RuntimeParticleInfo info = new RuntimeParticleInfo();
                    info.name = ps.gameObject.name;
                    info.go = ps.gameObject;
                    info.duration = ps.duration;
                    info.maxParticles = ps.maxParticles;
                    info.particleCount = ps.particleCount;
                    mParticleInfos.Add(info);

                }
                SortParticle(sortType);
                Repaint();
            }
            catch (Exception e)
            {
                collectGo();
            }
                
        }


    

        private void LastPage()
        {
            if (curPage == 0)
            {
                return;
            }
            curPage--;
        }


        private void NextPage()
        {

            if ((curPage + 1) == totalpageCnt)
            {
                return;
            }
            curPage++;
        }
    }
}
