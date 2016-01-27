using UnityEngine;
using System.Collections;

public class PositionMarker : MonoBehaviour
{
    public ScriptRuntime.Vector3 Position
    {
        get
        {
            Vector3 pos = transform.position;
            return new ScriptRuntime.Vector3(pos.x, pos.y, pos.z);
        }
    }
}
