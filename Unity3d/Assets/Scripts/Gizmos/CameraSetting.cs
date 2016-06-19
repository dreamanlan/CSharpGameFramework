using UnityEngine;
using System.Collections;

public class CameraSetting : MonoBehaviour {
    public float distance = 25;
    public float up = 1;
    public float yaw = 0;
    public float pitch = 35;

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(pitch, yaw, 0);
    }
}
