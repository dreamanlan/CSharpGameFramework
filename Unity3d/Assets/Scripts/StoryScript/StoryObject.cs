using UnityEngine;
using System.Collections;

public class StoryObject : MonoBehaviour
{
    public void SetLightVisible(int visible)
    {
        Light[] lights = gameObject.GetComponentsInChildren<Light>();
        for (int i = 0; i < lights.Length; ++i) {
            Light light = lights[i];
            if(visible==0){
                light.enabled = false;
            } else {
                light.enabled = true;
            }
        }
    }
    public void SetVisible(int visible)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; ++i) {
            if (visible == 0) {
                renderers[i].enabled = false;
            } else {
                renderers[i].enabled = true;
            }
        }
    }
    public void SetChildActive(object[] args)
    {
        if (args.Length < 2) return;
        string child = (string)args[0];
        int visible = (int)args[1];

        var t = transform.Find(child);
        if (null != t) {
            t.gameObject.SetActive(visible != 0);
        }
    }
    public void PlayAnimation(object[] args)
    {
        if (args.Length < 2) return;
        string animName = (string)args[0];
        float speed = (float)args[1];
        Animator[] animators = gameObject.GetComponentsInChildren<Animator>();
        if (null != animators) {
            for (int i = 0; i < animators.Length; ++i) {
                var animator = animators[i];
                animator.Play(animName, -1, 0);
                animator.speed = speed;
            }
        }
    }
    public void PlayParticle()
    {
        ParticleSystem[] pss = gameObject.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < pss.Length; i++) {
            pss[i].Play();
        }
    }
    public void StopParticle()
    {
        ParticleSystem[] pss = gameObject.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < pss.Length; i++) {
            pss[i].Stop();
        }
    }
    public void EnableObstacle(int arg)
    {
        bool enable = (int)arg != 0 ? true : false;
        var obs = gameObject.GetComponentInChildren<UnityEngine.AI.NavMeshObstacle>();
        if (null != obs) {
            obs.enabled = enable;
        }
    }
    public void PlaySound(int index)
    {
        AudioSource[] audios = gameObject.GetComponentsInChildren<AudioSource>();
        if (null != audios && index >= 0 && index < audios.Length) {
            audios[index].Play();
        }
    }
    public void StopSound(int index)
    {
        AudioSource[] audios = gameObject.GetComponentsInChildren<AudioSource>();
        if (null != audios && index >= 0 && index < audios.Length) {
            audios[index].Stop();
        }
    }
}
