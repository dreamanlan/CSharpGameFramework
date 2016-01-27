using UnityEngine;
using System.Collections;

public class ParticleRendererGetEnable : MonoBehaviour
{

    public bool mEnabled = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnBecameVisible()
    {
        mEnabled = true;
    }

    void OnBecameInvisible()
    {
        mEnabled = false;
    }
}
