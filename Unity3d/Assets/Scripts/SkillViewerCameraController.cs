using UnityEngine;
using System.Collections;
using GameFramework;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SkillViewerCameraController
{
    enum State
    {
        Lookat,     //盯着目标
        ToLookat,   //切换到盯着目标
        FixPose,    //固定点
        ToFixPose,  //切换到固定点
        Track,      //由外部控制摄像机轨迹
    }

    struct Pose
    {
        public Vector3 pos;
        public Quaternion rot;

        public static float GetScale(float speed, float angleSpeed, Pose p1, Pose p2)
        {
            float scale1 = (p1.pos - p2.pos).sqrMagnitude / (speed * speed);
            Quaternion q = p1.rot * Quaternion.Inverse(p2.rot);
            float angle;
            Vector3 axis;
            q.ToAngleAxis(out angle, out axis);
            float scale2 = (angle / angleSpeed);
            return scale1 > scale2 ? scale1 : scale2;
        }
    }

    private int followTargetId;

    private Pose followPose;
    private Pose fixPose;
    private Pose startPos;

    private float timeToStartSwitch = 0;
    private float timeToEndSwitch = 0;

    private State curState;
    private Camera camera;
    //public float defaultDistance = 24.0f;
    private SkillViewerCameraSetting cameraSetting;

    float shakeTrigerTime = 0.0f;
    float shakeRandomStart = 0.0f;
    bool cameraZooming = false;
    float lastZoomDistance = 0.0f;
    
    public void Update()
    {
        if (camera == null)
        {
            return;
        }

        #region hide this
        if (curState == State.Lookat)
        {
            camera.transform.position = followPose.pos;
        }
        else if (curState == State.FixPose)
        {
            camera.transform.position = fixPose.pos;
        }
        else if (curState == State.ToLookat)
        {
            float t = (Time.time - timeToStartSwitch) / (timeToEndSwitch - timeToStartSwitch);
            float t2 = Mathf.SmoothStep(0, 1, t);
            camera.transform.position = Vector3.Lerp(startPos.pos, followPose.pos, t2);
            camera.transform.rotation = Quaternion.Lerp(startPos.rot, followPose.rot, t2);
            Debug.Log("turn to follow:" + startPos.pos + "," + followPose.pos);
            if (Time.time > timeToEndSwitch)
            {
                SetState(State.Lookat);
            }
        }
        else if (curState == State.ToFixPose)
        {
            float t = (Time.time - timeToStartSwitch) / (timeToEndSwitch - timeToStartSwitch);
            float t2 = Mathf.SmoothStep(0, 1, t);
            camera.transform.position = Vector3.Lerp(startPos.pos, fixPose.pos, t2);
            camera.transform.rotation = Quaternion.Lerp(startPos.rot, fixPose.rot, t2);
            if (Time.time > timeToEndSwitch)
            {
                SetState(State.FixPose);
            }
        }
        #endregion
        
        if (followTargetId > 0)
        {
            GameObject obj = ClientModule.Instance.GetGameObject(followTargetId);
            if (null != obj)
            {
                if (cameraSetting.yaw > 180.0f) 
                {
                    cameraSetting.yaw -= 360.0f;
                }
                if (cameraSetting.yaw < -180.0f)
                {
                    cameraSetting.yaw += 360.0f;
                }

                Vector3 a = new Vector3(0.0f, cameraSetting.cameraPath_Near.x, cameraSetting.cameraPath_Near.y);
                Vector3 b = new Vector3(0.0f, cameraSetting.cameraPath_ControlPoint.x, cameraSetting.cameraPath_ControlPoint.y);
                Vector3 c = new Vector3(0.0f, cameraSetting.cameraPath_Far.x, cameraSetting.cameraPath_Far.y);

                // rotate matrix
                Matrix4x4 rotateMat = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(cameraSetting.yaw, Vector3.up), Vector3.one);
                // transform matrix for camera pos 
                Matrix4x4 mat = rotateMat * Matrix4x4.TRS(new Vector3(0.0f, cameraSetting.up, 0.0f), Quaternion.AngleAxis(cameraSetting.pitch, Vector3.right), Vector3.one) * Matrix4x4.TRS(-new Vector3(0.0f, cameraSetting.up, 0.0f), Quaternion.identity, Vector3.one);

                // calculate camera pos in world position
                followPose.pos = obj.transform.position + mat.MultiplyPoint(SkillViewerCameraSetting.Bezier2(a, b, c, cameraSetting.distance));
                //followPose.pos = followPose.pos + new Vector3(0, Mathf.Cos(Mathf.Deg2Rad*Time.timeSinceLevelLoad*10.0f), 0);
                // update camera position and rotation


                camera.transform.position = followPose.pos;
                Vector3 targetV3 = obj.transform.position+new Vector3(0,cameraSetting.up,0);

                camera.transform.LookAt(targetV3);

                Vector3 dir = followPose.pos-targetV3;
                float length = dir.magnitude;
                dir.Normalize();
                Ray ray = new Ray(targetV3, dir);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, 1000.0f)) 
                {
                    if (hitInfo.distance * 0.85f < length) 
                    {
                        Vector3 v3 = targetV3 + (hitInfo.distance*0.85f) * dir;//
                        camera.transform.position = v3;
                        camera.transform.LookAt(targetV3);
                    }
                }
                
                // debug start
                Debug.DrawLine(obj.transform.position, obj.transform.position + new Vector3(0.0f, cameraSetting.up, 0.0f));
                Debug.DrawLine(followPose.pos, obj.transform.position + new Vector3(0.0f, cameraSetting.up, 0.0f));

                Vector3 pos0 = obj.transform.position + mat.MultiplyPoint(a);
                Vector3 pos1 = obj.transform.position + mat.MultiplyPoint(b);
                Vector3 pos2 = obj.transform.position + mat.MultiplyPoint(c);

                Debug.DrawLine(pos0, pos1);
                Debug.DrawLine(pos1, pos2);
                
                SkillViewerCameraSetting.DrawBezier2Line(pos0, pos1, pos2);
                cameraSetting.targetRoot = obj.transform.position;
                // debug end
            }
            else
            {
                followTargetId = 0;
            }
        }

        Debug.DrawLine(followPose.pos, followPose.pos + new Vector3(0.0f, 1.0f, 0.0f));

        Vector2 cameraShakeOffset = Vector2.zero;
        if (cameraSetting.enableCameraShake) {
            cameraSetting.enableCameraShake = false;
            cameraSetting.inShaking = true;
            shakeTrigerTime = Time.realtimeSinceStartup;
            shakeRandomStart = Random.Range(-1000.0f, 1000.0f);
        }
        if (cameraSetting.inShaking) {
            float elapsed = Time.realtimeSinceStartup - shakeTrigerTime;
            if (elapsed < cameraSetting.shakeDuration) {
                float percentComplete = elapsed / cameraSetting.shakeDuration;

                float damper = 1.0f - Mathf.Clamp(2.0f * percentComplete - 1.0f, 0.0f, 1.0f);

                float alpha = shakeRandomStart + cameraSetting.shakeSpeed * percentComplete;

                // map to [-1, 1]
                float x = Mathf.PerlinNoise(alpha, 0.0f) * 2.0f - 1.0f;
                float y = Mathf.PerlinNoise(0.0f, alpha) * 2.0f - 1.0f;

                x *= cameraSetting.shakeMagnitude * damper;
                y *= cameraSetting.shakeMagnitude * damper;
                cameraShakeOffset = new Vector2(x, y) * (cameraSetting.distance + 0.1f);
            } else {
                cameraSetting.inShaking = false;
            }
        }
        if (cameraSetting.inShaking) {
            Vector3 o = camera.transform.right * cameraShakeOffset.x + camera.transform.up * cameraShakeOffset.y;
            camera.transform.position += o;
        }
    }
    
    public void MoveToLookat(Vector3 target,bool bSmoothMove) {
        followPose.pos = target + cameraSetting.GetCurrentBezierCameraOffset();
        followPose.rot = cameraSetting.GetRotation();
        startPos.pos = camera.transform.position;
        startPos.rot = camera.transform.rotation;
        SetState(State.ToLookat);
        timeToStartSwitch = Time.time;
        timeToEndSwitch = timeToStartSwitch + Pose.GetScale(20.0f, 90.0f, startPos, followPose);
    }
    public void MoveToFixedPosition(Vector3 targetPos, Quaternion targetRot, bool bSmoothMove)
    {
        fixPose.pos = targetPos;
        fixPose.rot = cameraSetting.GetRotation();
        startPos.pos = camera.transform.position;
        startPos.rot = camera.transform.rotation;
        SetState(State.ToFixPose);
        
        timeToStartSwitch = Time.time;
        timeToEndSwitch = timeToStartSwitch + Pose.GetScale(20.0f, 90.0f, startPos, fixPose);
    }
    public void ToLastLookat()
    {
        followPose.rot = cameraSetting.GetRotation();
        startPos.pos = camera.transform.position;
        startPos.rot = camera.transform.rotation;
        SetState(State.ToLookat);
        timeToStartSwitch = Time.time;
        timeToEndSwitch = timeToStartSwitch + Pose.GetScale(20.0f, 90.0f, startPos, followPose);
    }
    public void ToTrack()
    {
        SetState(State.Track);
    }

    public void ClearFollow()
    {
        followTargetId = 0;
    }
    public void Follow(int targetId)
    {
        followTargetId = targetId;
    }

    public void Shake(float duration, float speed, float magnitude)
    {
        cameraSetting.shakeDuration = duration;
        cameraSetting.shakeSpeed = speed;
        cameraSetting.shakeMagnitude = magnitude;
        cameraSetting.enableCameraShake = true;
    }

    private void SetState(State st)
    {
        Debug.Log("st:" + st);
        curState = st;
    }
    
    public void OnLevelWasLoaded(TableConfig.Level level)
    {
        camera = Camera.main;
        cameraSetting = camera.gameObject.GetComponent<SkillViewerCameraSetting>();
        if (cameraSetting == null) {
            cameraSetting = camera.gameObject.AddComponent<SkillViewerCameraSetting>();
        }
        //camera.transform.rotation = cameraSetting.GetRotation();// 注释掉，临时让摄像机设置不起作用 peterdou

        SetState(State.Lookat);
        followPose.pos = camera.transform.position;
        followPose.rot = camera.transform.rotation;
        fixPose.pos = camera.transform.position;
        fixPose.rot = camera.transform.rotation;
    }
}
