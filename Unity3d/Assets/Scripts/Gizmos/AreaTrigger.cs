using UnityEngine;
using System.Collections;

public class AreaTrigger : MonoBehaviour
{
    public int groupIndex = 0;
    public void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = new Color(0,0,1,0.4f);
        Gizmos.DrawCube(Vector3.zero, new Vector3(1, 1, 1));
        Gizmos.color = new Color(1, 1, 1, 1);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(1, 1, 1));
    }
}
