using UnityEngine;
using System.Collections;

public class SkillViewerCameraConfigor : MonoBehaviour {

    private Transform rootNode;
    private Transform node0;
    private Transform node1;
    private Transform node2;

    private Transform cam_node;
    private Transform target_node;

    [Range(0.0f, 1.0f)]
    public float lerpDegree;
	// Use this for initialization
    [Range(-70.0f, 60.0f)]
    public float camera_pitch;

    SkillViewerCameraSetting cameraSetting;

	void Start () {
	
	}
	

	public void OnDrawGizmos(){

        
        if (node0 == null) 
        {
            GameObject n0 = GameObject.Find("camera_nearControl");
            if(n0!=null)node0 = n0.transform;
        }
        if (node1 == null)
        {
            GameObject n1 = GameObject.Find("camera_midControl");
            if (n1 != null) node1 = n1.transform;
        }
        if (node2 == null)
        {
            GameObject n2 = GameObject.Find("camera_farControl");
            if (n2 != null) node2 = n2.transform;
        }

        if (cam_node == null)
        {
            GameObject nc = GameObject.Find("Main Camera");
            if (nc != null) cam_node = nc.transform;
        }

        if (target_node == null) 
        {
            GameObject go = GameObject.Find("camera_target");
            if (go != null) target_node = go.transform;
        }

        Matrix4x4 mat = Matrix4x4.TRS(target_node.position, Quaternion.AngleAxis(camera_pitch, Vector3.right), Vector3.one)*Matrix4x4.TRS(-target_node.position,Quaternion.identity,Vector3.one);

        node0.localPosition = new Vector3(0.0f, node0.localPosition.y, node0.localPosition.z);
        node1.localPosition = new Vector3(0.0f, node1.localPosition.y, node1.localPosition.z);
        node2.localPosition = new Vector3(0.0f, node2.localPosition.y, node2.localPosition.z);

        target_node.localPosition = new Vector3(0.0f, target_node.localPosition.y, 0.0f);

        if (target_node != null) 
        {
            GameFramework.Utility.DrawGizmosCircle(target_node.position, 0.5f);
        }
        if ((node0 != null && node1 != null))
        {
            Debug.DrawLine(node0.position, node1.position);
            Debug.DrawLine(mat.MultiplyPoint(node0.position), mat.MultiplyPoint(node1.position));
        }
        if ((node2 != null && node1 != null))
        {
            Debug.DrawLine(node1.position, node2.position);
            Debug.DrawLine(mat.MultiplyPoint(node1.position), mat.MultiplyPoint(node2.position));
        }
        if ((node0 != null) && (node1 != null) && (node2 != null)) 
        {
            SkillViewerCameraSetting.DrawBezier2Line(node0.position, node1.position, node2.position);//DrawBezier2Line
            SkillViewerCameraSetting.DrawBezier2Line(mat.MultiplyPoint(node0.position), mat.MultiplyPoint(node1.position), mat.MultiplyPoint(node2.position));//DrawBezier2Line
        }

        Vector3 un_rotate_node = SkillViewerCameraSetting.Bezier2(node0.position, node1.position, node2.position, lerpDegree);
        Debug.DrawLine(un_rotate_node, target_node.position);


        //cam_node.position = mat.MultiplyPoint(CameraSetting.Bezier2(node0.position, node1.position, node2.position, lerpDegree));
        //cam_node.LookAt(target_node.position);
        Debug.DrawLine(cam_node.position, target_node.position);
        
	}
}
