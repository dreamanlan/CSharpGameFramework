using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScriptableFramework;
using ScriptableFramework.Plugin;
using ScriptableFramework.Skill;
using ScriptableFramework.Skill.Trigers;
using DotnetSkillScript;

public class BulletManager
{
    public GameObject GetCollideObject(GameObject bullet)
    {
        var pos1 = bullet.transform.position;
        foreach (var pair in m_Bullets) {
            var other = pair.Value;
            var otherObj = other.Obj;
            if (bullet != otherObj) {
                var pos2 = otherObj.transform.position;
                var lastPos2 = other.GetLastPos();
                var distSqr = (pos1 - pos2).sqrMagnitude;
                if (distSqr < 0.01f) {
                    UnityEngine.Debug.Log("distSqr<0.01f, collide.");
                    return otherObj;
                } else {
                    UnityEngine.Debug.Log(string.Format("Dist:{0}",distSqr));
                }
                var dot = Vector3.Dot(lastPos2 - pos2, pos1 - pos2);
                if (lastPos2.sqrMagnitude > Geometry.c_FloatPrecision && dot > 0) {
                    UnityEngine.Debug.Log("cross, collide.");
                    return otherObj;
                } else {
                    UnityEngine.Debug.Log(string.Format("lastPos2Sqr:{0} dot:{1}", lastPos2.sqrMagnitude, dot));
                }
            }
        }
        return null;
    }
    public void UpdatePos(GameObject bullet)
    {
        CollideInfo info;
        if (m_Bullets.TryGetValue(bullet.GetInstanceID(), out info)) {
            info.SetLastPos(bullet.transform.position);
        }
    }
    public void AddBullet(GameObject bullet)
    {
        m_Bullets.Add(bullet.GetInstanceID(), new CollideInfo { Obj = bullet });
    }
    public void RemoveBullet(GameObject bullet)
    {
        m_Bullets.Remove(bullet.GetInstanceID());
    }
    
    internal class CollideInfo
    {
        internal Vector3 LastPos1 = Vector3.zero;
        internal Vector3 LastPos2 = Vector3.zero;
        internal GameObject Obj = null;
        internal int LastSetIndex = 0;

        internal Vector3 GetLastPos()
        {
            if (LastSetIndex == 0) {
                return LastPos2;
            } else {
                return LastPos1;
            }
        }
        internal void SetLastPos(Vector3 pos)
        {
            LastSetIndex = (LastSetIndex + 1) % 2;
            if (LastSetIndex == 0) {
                LastPos1 = pos;
            } else {
                LastPos2 = pos;
            }
        }
    }

    private Dictionary<int, CollideInfo> m_Bullets = new Dictionary<int, CollideInfo>();

    public static BulletManager Instance
    {
        get
        {
            return s_Instance;
        }
    }
    private static BulletManager s_Instance = new BulletManager();
}
