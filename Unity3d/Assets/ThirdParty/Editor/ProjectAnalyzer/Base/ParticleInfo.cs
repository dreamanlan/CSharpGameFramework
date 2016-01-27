using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ProjectAnalyzer
{
    class ParticleInfo : ResInfo
    {
        public enum SortType
        {
            Name,
            ParentIsParticle,
            ParticleSysCnt,
            Duration,
            MaxParticles,
            MinParticles,
            Conllier,
            Propose
        }


        public bool parentIsParticle = false;
        public int particleSysCnt = 0;
        public float duration = 0;
        public int maxParticles = 0;
        public int minParticles = int.MaxValue;
        public bool conllier = false;
        public ParticleInfo(GameObject go, string path)
        {

            this.name = go.name;
            this.path = path;
            ParticleSystem psParent = go.GetComponent<ParticleSystem>();
            this.parentIsParticle = (psParent != null);
            if (psParent != null)
            {
                this.conllier = psParent.GetComponent<Collider>() != null;
            }

            ParticleSystem[] pses = go.GetComponentsInChildren<ParticleSystem>();
            this.particleSysCnt = pses.Length;
            foreach (ParticleSystem ps in pses)
            {
                if (this.parentIsParticle != null && psParent.Equals(ps))
                {
                    continue;
                }
                if (ps.duration > duration)                
                {
                    this.duration = ps.duration;
                }
                if (ps.maxParticles > maxParticles)
                {
                    this.maxParticles = ps.maxParticles;
                }
                if (ps.maxParticles < this.minParticles)
                {
                    this.minParticles = ps.maxParticles;
                }
                if(this.conllier == false){
                    this.conllier = ps.GetComponent<Collider>() != null && ps.GetComponent<Collider2D>() != null;
                }
            }
            
        }

    }
}
