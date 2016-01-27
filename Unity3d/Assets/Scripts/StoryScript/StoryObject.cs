using UnityEngine;
using System.Collections;

public class StoryObject : MonoBehaviour
{
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
    public void PlayAnimation(object[] args)
    {
        if (args.Length < 2) return;
        string animName = (string)args[0];
        float speed = (float)args[1];
        Animator animator = gameObject.GetComponent<Animator>();
        if (null != animator) {
            animator.Play(animName);
            animator.speed = speed;
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
    public void PlaySound(int index)
    {
        AudioSource[] audios = gameObject.GetComponents<AudioSource>();
        if (null != audios && index >= 0 && index < audios.Length) {
            audios[index].Play();
        }
    }
    public void StopSound(int index)
    {
        AudioSource[] audios = gameObject.GetComponents<AudioSource>();
        if (null != audios && index >= 0 && index < audios.Length) {
            audios[index].Stop();
        }
    }
}
