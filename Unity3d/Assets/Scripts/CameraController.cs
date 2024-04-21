using UnityEngine;
using System.Collections;
using GameFramework;

public class CameraController
{
    enum State
    {
        FollowPath,//follow the path
        FollowToFixPose,//Convert from following path to fixed point
        FixPose,//fixed point
        FixPoseToFollow,//fixed point to follow
    }

    struct Pose
    {
        public Vector3 pos;
        public Quaternion rot;
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
    private CameraSetting cameraSetting;

    public CameraController(Camera cam) {
        cameraSetting = cam.gameObject.GetComponent<CameraSetting>();
        if (cameraSetting == null) {
            cameraSetting = cam.gameObject.AddComponent<CameraSetting>();
        }
        //cameraSetting.yaw = cam.gameObject.transform.rotation.eulerAngles.y;
        //cameraSetting.pitch = cam.gameObject.transform.rotation.eulerAngles.x;
        cam.transform.rotation = cameraSetting.GetRotation();

        SetState(State.FollowPath);
        followPose.pos = cam.transform.position;
        followPose.rot = cam.transform.rotation;
        fixPose.pos = cam.transform.position;
        fixPose.rot = cam.transform.rotation;
        camera = cam;        
    }


    public void Update() {
        if (curState == State.FollowPath)
        {
            camera.transform.position = followPose.pos;
        }
        else if (curState == State.FixPose)
        {
            camera.transform.position = fixPose.pos;
        }
        else if (curState == State.FixPoseToFollow)
        {
            float t = (Time.time - timeToStartSwitch) / (timeToEndSwitch - timeToStartSwitch);
            float t2 = Mathf.SmoothStep(0, 1, t);
            camera.transform.position = Vector3.Lerp(startPos.pos, followPose.pos, t2);
            Debug.Log("turn to follow:" + startPos.pos + "," + followPose.pos);
            if (Time.time > timeToEndSwitch)
            {
                SetState(State.FollowPath);
            }
        }
        else if (curState == State.FollowToFixPose)
        {
            float t = (Time.time - timeToStartSwitch) / (timeToEndSwitch - timeToStartSwitch);
            float t2 = Mathf.SmoothStep(0, 1, t);
            camera.transform.position = Vector3.Lerp(startPos.pos, fixPose.pos, t2);
            if (Time.time > timeToEndSwitch)
            {
                SetState(State.FixPose);
            }
        }


        if (followTargetId > 0)
        {
            GameObject obj = PluginFramework.Instance.GetGameObject(followTargetId);
            if (null != obj)
            {
                followPose.pos = obj.transform.position + Vector3.up * cameraSetting.up - camera.transform.rotation * Vector3.forward * cameraSetting.distance;
            }
            else
            {
                followTargetId = 0;
            }
        }
        camera.transform.rotation = cameraSetting.GetRotation();
    }

    //Move on the scene, use it in normal state and when sliding the screen with your finger
    public void MoveFollowPath(Vector3 target,bool bSmoothMove) {
        followPose.pos = target + Vector3.up * 1.5f - followPose.rot * Vector3.forward * cameraSetting.distance;
        ToFollowPath();
    }


    //Smooth movement to fixed point.
    public void MoveToFixedPosition(Vector3 targetPos,Quaternion targetRot, bool bSmoothMove) {
        //followPose.pos = targetPos + Vector3.up * 1.5f - followPose.rot * Vector3.forward * cameraSetting.distance;
        //followPose.rot = targetRot;
        //if (curState == State.FollowPath) {
            SetState(State.FollowToFixPose);
            startPos.pos = camera.transform.position;
            startPos.rot = camera.transform.rotation;
            timeToStartSwitch = Time.time;
            timeToEndSwitch = timeToStartSwitch + 1.0f;
            fixPose.pos = targetPos;
        //}
    }

    //Move smoothly to follow state
    public void ToFollowPath()
    {
        //if (curState == State.FixPose) {
            startPos.pos = camera.transform.position;
            startPos.rot = camera.transform.rotation;
            SetState(State.FixPoseToFollow);
            timeToStartSwitch = Time.time;
            timeToEndSwitch = timeToStartSwitch + 1.0f;
        //}
    }

    public void ClearFollow()
    {
        followTargetId = 0;
    }
    public void Follow(int targetId)
    {
        followTargetId = targetId;
    }

    private void SetState(State st)
    {
        Debug.Log("st:" + st);
        curState = st;
    }
}
