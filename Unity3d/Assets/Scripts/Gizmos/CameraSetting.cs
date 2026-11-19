using UnityEngine;
using System.Collections;

public class CameraSetting : MonoBehaviour {
    // Spectator-style view for the 512x512 world tiles. Scenes already have
    // serialized CameraSetting values, so CameraController applies these at
    // runtime; keep in sync with AppEditorDslApi.Maps.cs (map_camera_fix).
    public const float BattleDistance = 104f;
    // up shifts the LOOK-AT point above the leader (camPos = leader + up*Y
    // - rot*forward*distance), so a non-zero value pushes the player down the
    // screen. Keep it at 0 to centre the player; camera height still comes
    // from distance*sin(pitch) (~75 units here). With up=0, camDist == distance.
    public const float BattleUp = 0f;
    public const float BattlePitch = 46f;

    public float distance = BattleDistance;
    public float up = BattleUp;
    public float yaw = 0;
    public float pitch = BattlePitch;

    /// <summary>Force the spectator pose, overriding serialized scene values.</summary>
    public void ApplyBattleView()
    {
        distance = BattleDistance;
        up = BattleUp;
        yaw = 0f;
        pitch = BattlePitch;
    }

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(pitch, yaw, 0);
    }
}
