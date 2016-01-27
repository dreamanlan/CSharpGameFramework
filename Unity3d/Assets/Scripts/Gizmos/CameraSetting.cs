using UnityEngine;
using System.Collections;

public class CameraSetting : MonoBehaviour {
    public float distance = 20;
    public float up = 2;
    public float yaw = 0;
    public float pitch = 45;

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(pitch, yaw, 0);
    }
}
