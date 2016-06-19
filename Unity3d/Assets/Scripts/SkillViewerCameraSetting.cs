using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillViewerCameraSetting : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float distance = 0.5f;
    public float up = 3.5f;
    public float yaw = 208.7f;
    [Range(-70.0f, 60.0f)]
    public float pitch = 27;
    [SerializeField]
    public Vector2 cameraPath_Near;
    [SerializeField]
    public Vector2 cameraPath_Far;    
    public Vector2 cameraPath_ControlPoint;

    public float shakeDuration = 0.3f;
    public float shakeSpeed = 15.0f;
    public float shakeMagnitude = 1.0f;

    public bool enableCameraShake = false;
    [HideInInspector]
    public bool inShaking = false;
    [HideInInspector]
    public Vector3 targetRoot;
    public Quaternion GetRotation()
    {
        GameObject obj = new GameObject();
        obj.transform.position = GetCurrentBezierCameraOffset() - new Vector3(0, up, 0);
        obj.transform.LookAt(Vector3.zero);
        return obj.transform.rotation;
    }
    public Vector3 GetCurrentBezierCameraOffset()
    {
        Vector3 a = new Vector3(0.0f, cameraPath_Near.x, cameraPath_Near.y);
        Vector3 b = new Vector3(0.0f, cameraPath_ControlPoint.x, cameraPath_ControlPoint.y);
        Vector3 c = new Vector3(0.0f, cameraPath_Far.x, cameraPath_Far.y);

        // rotate matrix
        Matrix4x4 rotateMat = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(yaw, Vector3.up), Vector3.one);
        // transform matrix for camera pos 
        Matrix4x4 mat = rotateMat * Matrix4x4.TRS(new Vector3(0.0f, up, 0.0f), Quaternion.AngleAxis(pitch, Vector3.right), Vector3.one) * Matrix4x4.TRS(-new Vector3(0.0f, up, 0.0f), Quaternion.identity, Vector3.one);

        // calculate camera pos in world position
        return mat.MultiplyPoint(Bezier2(a, b, c, distance));
    }
    public void OnDrawGizmos()
    {

    }
    public static void DrawBezier2Line(Vector3 s, Vector3 p, Vector3 e)
    {
        for (int i = 0; i < 100; i++)
        {
            float t0_d = i * 0.01f;
            float t1_d = (i + 1) * 0.01f;
            Vector3 v0 = Bezier2(s, p, e, t0_d);
            Vector3 v1 = Bezier2(s, p, e, t1_d);
            Debug.DrawLine(v0, v1);
        }
    }
    public static Vector3 Bezier2(Vector3 s, Vector3 p, Vector3 e, float t)
    {
        float rt = 1 - t;
        return rt * rt * s + 2 * rt * t * p + t * t * e;
    }
}
