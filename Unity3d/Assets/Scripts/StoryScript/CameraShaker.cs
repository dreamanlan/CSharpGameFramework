using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraShaker : MonoBehaviour
{
    public enum ShakeDir
    {
        xyDir = 0,
        zDir = 1,
        yDir = 2,
    };
    public ShakeDir shake_Direction = ShakeDir.xyDir;
    public AnimationCurve camShake_Hit;
    public AnimationCurve[] camShake_Hits;
    public bool toogleCamerShake = false;
    public float shakeDuration = 1.0f;
    public float camShakeamplifier = 1.0f;
    public int currentShakeIndex = 0;

    private bool inShake = false;
    private float shakeStartTime = 0.0f;
    private float _theta = 0.0f;

    private Transform camTransform;
    private Transform parentTransform;
    
	// Use this for initialization
	void Start () {
        camTransform = GetComponent<Transform>();
        parentTransform = camTransform.parent;
	}
	
	// Update is called once per frame
    void LateUpdate()
    {
        if (toogleCamerShake) 
        {
            toogleCamerShake = false;
            
            shakeStartTime = Time.timeSinceLevelLoad;
            if (!inShake) 
            {
                //originalPos = camTransform.position;//.localPosition;            
            }
            _theta =  Random.Range(0.0f, Mathf.PI * 2);

            inShake = true;

            if (currentShakeIndex < camShake_Hits.Length) 
            {
                camShake_Hit = camShake_Hits[currentShakeIndex]; 
            }
        }
        if (inShake) 
        {   
            float currentTime = Time.timeSinceLevelLoad;
            if (currentTime - shakeStartTime <= shakeDuration)
            {
                float lerpValue = (currentTime - shakeStartTime) / shakeDuration;
                float offSet = camShake_Hit.Evaluate(lerpValue);
                float offSet1 = camShake_Hit.Evaluate(lerpValue+0.1f);
                
                Vector3 offsetV3 = Vector3.zero;
                if (shake_Direction == ShakeDir.xyDir) 
                {
                    offsetV3 = Mathf.Cos(_theta) * offSet * camShakeamplifier * parentTransform.right +Mathf.Sin(_theta) * offSet * camShakeamplifier * parentTransform.up;
                    offsetV3 += Mathf.Cos(_theta + Mathf.PI * 0.5f) * offSet1 * camShakeamplifier * camTransform.right + Mathf.Sin(_theta + Mathf.PI * 0.5f) * offSet1 * camShakeamplifier * camTransform.up;
                    offsetV3 *= 0.5f;
                }
                else if (shake_Direction == ShakeDir.zDir)
                {
                    offsetV3 = -offSet * camShakeamplifier * parentTransform.forward;
                }
                else if (shake_Direction == ShakeDir.yDir)
                {
                    offsetV3 =  offSet * camShakeamplifier * parentTransform.up;
                    offsetV3 += offSet1 * camShakeamplifier * camTransform.up;
                    offsetV3 *= 0.5f;
                }
                camTransform.localPosition = offsetV3;
            }
            else 
            {
                camTransform.localPosition = Vector3.zero;
                inShake = false;
                OnExitShake();
            }
        }
	}

    public void CancelShake()
    {
        shakeStartTime = 0;
    }

    public void OnExitShake() 
    {
    }

    public void Shake(float duration, int shakeTpye, float magnitude, int shakeAnimationCurveIndex, bool enableRadialBlur)
    {
        //enableRadialBlur = true;
        if (enableRadialBlur)
        {
            Shake(duration, shakeTpye, magnitude, shakeAnimationCurveIndex, enableRadialBlur, new Vector2(0.5f, 0.5f));
        }
        else
        {
            Shake(duration, shakeTpye, magnitude, shakeAnimationCurveIndex, enableRadialBlur, Vector2.zero);
        }
    }

    public void Shake(float duration, int shakeTpye, float magnitude, int shakeAnimationCurveIndex, bool enableRadialBlur, Vector2 pos)
    {
        shakeDuration = duration;
        shake_Direction = (ShakeDir)shakeTpye;
        camShakeamplifier = magnitude;
        currentShakeIndex = (int)(shakeAnimationCurveIndex);
        toogleCamerShake = true;
    }
    
    public void Shake(float duration, int shakeTpye, float magnitude, int shakeAnimationCurveIndex)
    {
        Shake(duration, shakeTpye, magnitude, shakeAnimationCurveIndex, false);
    }

    private void CameraShake(object[] args)
    {
        if (null != args && args.Length >= 4) {
            float a1 = (float)System.Convert.ChangeType(args[0], typeof(float));
            int a2 = (int)System.Convert.ChangeType(args[1], typeof(int));
            float a3 = (float)System.Convert.ChangeType(args[2], typeof(float));
            int a4 = (int)System.Convert.ChangeType(args[3], typeof(int));
            Shake(a1, a2, a3, a4);
        }
    }
}
