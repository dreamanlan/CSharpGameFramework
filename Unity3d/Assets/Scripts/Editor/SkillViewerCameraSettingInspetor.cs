using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SkillViewerCameraSetting))]
public class CameraConfigInspedtor : Editor
{

    bool editingCamera = false;
    SkillViewerCameraSetting  cameraSetting;
    public void OnEnable()
    {
        cameraSetting = (SkillViewerCameraSetting)target;
    }


    private bool CheckEditing() 
    {
        GameObject cam_config = GameObject.Find("CameraConfigor");
        if (cam_config != null) { return true; }
        return false;
    }

    public override void OnInspectorGUI()//
    {
        

        DrawDefaultInspector();
        editingCamera = CheckEditing();
        if (editingCamera)
        {
            if (GUILayout.Button("结束曲线编辑"))
            {
                GameObject cam_config = GameObject.Find("CameraConfigor");
                ;

                /* save camera setting from  CameraConfigor to CameraSetting */
                SkillViewerCameraConfigor cc = cam_config.GetComponent<SkillViewerCameraConfigor>();
                cameraSetting.distance = cc.lerpDegree;
                cameraSetting.pitch = cc.camera_pitch;


                GameObject nearPoint = GameObject.Find("camera_nearControl");
                Vector3 nearP = nearPoint.transform.localPosition;
                cameraSetting.cameraPath_Near = new Vector2(nearP.y,nearP.z);

                GameObject midPoint = GameObject.Find("camera_midControl");
                Vector3 midP = midPoint.transform.localPosition;
                cameraSetting.cameraPath_ControlPoint = new Vector2(midP.y, midP.z);

                GameObject farPoint = GameObject.Find("camera_farControl");
                Vector3 farP = farPoint.transform.localPosition;
                cameraSetting.cameraPath_Far = new Vector2(farP.y,farP.z);

                GameObject targetPoint = GameObject.Find("camera_target");
                Vector3 targetP = targetPoint.transform.localPosition;
                cameraSetting.up = targetP.y;


                if (cam_config != null)
                {
                    GameObject.DestroyImmediate(cam_config);
                }

                //editingCamera = false;
            }
        }
        else 
        {
            if (GUILayout.Button("开始曲线编辑")) 
            {
                
                GameObject cam_config = GameObject.Find("CameraConfigor");
                if (cam_config == null) 
                {


                    cam_config = new GameObject("CameraConfigor");
                    SkillViewerCameraConfigor cc= cam_config.AddComponent<SkillViewerCameraConfigor>();
                    cc.camera_pitch = cameraSetting.pitch;
                    cc.lerpDegree = cameraSetting.distance;


                    cam_config.transform.rotation = Quaternion.AngleAxis(cameraSetting.yaw, Vector3.up);
                    cam_config.transform.position = cameraSetting.targetRoot;

                    GameObject son0 = new GameObject("camera_nearControl");
                    son0.transform.parent = cam_config.transform;
                    son0.transform.localPosition = new Vector3(0.0f, cameraSetting.cameraPath_Near.x, cameraSetting.cameraPath_Near.y);

                    GameObject son1 = new GameObject("camera_midControl");
                    son1.transform.parent = cam_config.transform;
                    son1.transform.localPosition = new Vector3(0.0f, cameraSetting.cameraPath_ControlPoint.x, cameraSetting.cameraPath_ControlPoint.y);
                    

                    GameObject son2 = new GameObject("camera_farControl");
                    son2.transform.parent = cam_config.transform;
                    son2.transform.localPosition = new Vector3(0.0f,cameraSetting.cameraPath_Far.x,cameraSetting.cameraPath_Far.y);


                    GameObject target = new GameObject("camera_target");
                    target.transform.parent = cam_config.transform;
                    target.transform.localPosition = new Vector3(0.0f, cameraSetting.up, 0.0f);



                    /* load camera setting from CameraSetting to CameraConfigor */
                }


                //editingCamera = true;
            }

            /*
            if (GUILayout.Button("保存当前摄像机配置")) 
            {
            
            }
             */
        }
    }
}
