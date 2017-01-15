using UnityEngine;
using System.Collections;

public class CameraSetting : MonoBehaviour {
    public float distance = 60;
    public float up = 10;
    public float yaw = 0;
    public float pitch = 60;

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(pitch, yaw, 0);
    }
}
