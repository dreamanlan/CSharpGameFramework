using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameFramework;
using GameFramework.Plugin;
using GameFramework.Skill;
using GameFramework.Skill.Trigers;
using SkillSystem;
using ScriptRuntime;

public class BulletManager
{
    public EntityInfo GetCollideObject(EntityInfo bullet)
    {
        var pos1 = bullet.GetMovementStateInfo().GetPosition3D();
        foreach (var pair in m_Bullets) {
            var other = pair.Value;
            var otherObj = other.Obj;
            if (bullet != otherObj) {
                var pos2 = otherObj.GetMovementStateInfo().GetPosition3D();
                var lastPos2 = other.GetLastPos();
                var distSqr = (pos1 - pos2).LengthSquared();
                if (distSqr < 0.01f) {
                    LogSystem.Info("distSqr<0.01f, collide.");
                    return otherObj;
                } else {
                    LogSystem.Info(string.Format("Dist:{0}", distSqr));
                }
                var dot = Vector3.Dot(lastPos2 - pos2, pos1 - pos2);
                if (lastPos2.LengthSquared() > Geometry.c_FloatPrecision && dot > 0) {
                    LogSystem.Info("cross, collide.");
                    return otherObj;
                } else {
                    LogSystem.Info(string.Format("lastPos2Sqr:{0} dot:{1}", lastPos2.LengthSquared(), dot));
                }
            }
        }
        return null;
    }
    public void UpdatePos(EntityInfo bullet)
    {
        CollideInfo info;
        if (m_Bullets.TryGetValue(bullet.GetId(), out info)) {
            info.SetLastPos(bullet.GetMovementStateInfo().GetPosition3D());
        }
    }
    public void AddBullet(EntityInfo bullet)
    {
        m_Bullets.Add(bullet.GetId(), new CollideInfo { Obj = bullet });
    }
    public void RemoveBullet(EntityInfo bullet)
    {
        m_Bullets.Remove(bullet.GetId());
    }
    
    internal class CollideInfo
    {
        internal Vector3 LastPos1 = Vector3.Zero;
        internal Vector3 LastPos2 = Vector3.Zero;
        internal EntityInfo Obj = null;
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
