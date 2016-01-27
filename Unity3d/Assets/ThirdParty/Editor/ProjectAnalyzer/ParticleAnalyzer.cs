using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ProjectAnalyzer
{
    class ParticleAnalyzer : BaseAnalyzer
    {
        private Vector2 scrollPosTexture = new Vector2(0, 0);
        private ActiveSubType actSubTypeTexture = ActiveSubType.Details;
        private string[] toolStrings = { LanguageCfg.DETAILS, LanguageCfg.HELPS, LanguageCfg.SETTINGS };


        public void DrawParticles()
        {
            GUILayout.Space(10);
            actSubTypeTexture = (ActiveSubType)GUILayout.Toolbar((int)actSubTypeTexture, toolStrings, GUILayout.MaxWidth(200));
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format(LanguageCfg.PROJECT_PARTICLE_LIST, ProjectResource.Instance.particles.Count), GUILayout.Width(150));
            if (GUILayout.Button(LanguageCfg.PARTICLES_REFRESH, GUILayout.Width(120)))
            {
                ProjectResource.Instance.CollectParticles();
            }



            DrawPageCnt(ProjectResource.Instance.particles.Count);

            GUILayout.EndHorizontal();

             scrollPosTexture = EditorGUILayout.BeginScrollView(scrollPosTexture);
             if (actSubTypeTexture == ActiveSubType.Details)
             {

                 //绘制title
                 GUILayout.BeginHorizontal();
                 if (GUILayout.Button(LanguageCfg.NAME, GUILayout.Width(150)))
                 {
                     mCurPage = 0;
                     ProjectResource.Instance.SortParticle(ParticleInfo.SortType.Name);
                 }
                 if (GUILayout.Button(LanguageCfg.PARTICLE_SYS_PARENT_IS_PARTICLE, GUILayout.Width(100)))
                 {
                     mCurPage = 0;
                     ProjectResource.Instance.SortParticle(ParticleInfo.SortType.ParentIsParticle);
                 }
                 if (GUILayout.Button(LanguageCfg.PARTICLE_SYS_CNT, GUILayout.Width(100)))
                 {
                     mCurPage = 0;
                     ProjectResource.Instance.SortParticle(ParticleInfo.SortType.ParticleSysCnt);
                 }
                 if (GUILayout.Button(LanguageCfg.PARTICLE_SYS_MAX_DURATION, GUILayout.Width(100)))
                 {
                     mCurPage = 0;
                     ProjectResource.Instance.SortParticle(ParticleInfo.SortType.Duration);
                 }
                 if (GUILayout.Button(LanguageCfg.PARTICLE_SYS_MAX_PARTICLES, GUILayout.Width(100)))
                 {
                     mCurPage = 0;
                     ProjectResource.Instance.SortParticle(ParticleInfo.SortType.MaxParticles);
                 }
                 if (GUILayout.Button(LanguageCfg.PARTICLE_SYS_MIN_PARTICLES, GUILayout.Width(100)))
                 {
                     mCurPage = 0;
                     ProjectResource.Instance.SortParticle(ParticleInfo.SortType.MinParticles);
                 }
                 //if (GUILayout.Button(LanguageCfg.PARTICLE_SYS_MIN_COLIDER, GUILayout.Width(100)))
                 //{
                 //    ProjectResource.Instance.SortParticle(ParticleInfo.SortType.Conllier);
                 //}

                 GUILayout.EndHorizontal();

                 int start = mPageCnt * mCurPage;
                 int end = mPageCnt * (mCurPage + 1);
                 end = end >= ProjectResource.Instance.particles.Count ? ProjectResource.Instance.particles.Count : end;
                 for (int i = start; i < end; i++)
                 {
                     ParticleInfo info = ProjectResource.Instance.particles[i];
                     GUILayout.BeginHorizontal();
                     if (GUILayout.Button(info.name, GUILayout.Width(150)))
                     {
                         EditTools.PingAssetInProject(info.path);
                     }
                     GUILayout.Label(info.parentIsParticle.ToString(), GUILayout.Width(100));
                     GUILayout.Label(info.particleSysCnt.ToString(), GUILayout.Width(100));
                     GUILayout.Label(info.duration.ToString(), GUILayout.Width(100));
                     GUILayout.Label(info.maxParticles.ToString(), GUILayout.Width(100));
                     GUILayout.Label(info.minParticles.ToString(), GUILayout.Width(100));
                     //GUILayout.Label(info.conllier.ToString(), GUILayout.Width(100));
                     GUILayout.EndHorizontal();
                 }
             }
             else if (actSubTypeTexture == ActiveSubType.Helps)
             {
                 DrawHelpTips(LanguageCfg.HELP_PARTICLE);
             }
             EditorGUILayout.EndScrollView();
        }
    }
}
