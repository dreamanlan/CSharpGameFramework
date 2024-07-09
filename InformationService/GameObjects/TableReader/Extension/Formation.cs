using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptRuntime;
using ScriptableFramework;

namespace TableConfig
{
    public sealed partial class Formation
    {
        public struct PosDir
        {
            public float X;
            public float Y;
            public float Z;
            public float Dir;

            public PosDir(List<float> coords, float dir)
            {
                if (coords.Count == 3) {
                    X = coords[0];
                    Y = coords[1];
                    Z = coords[2];
                    Dir = dir;
                } else {
                    X = 0;
                    Y = 0;
                    Z = 0;
                    Dir = 0;
                }
            }
            public Vector3 CalcPosDir(Vector3 leaderPos, float leaderDir, out float dir)
            {
                Vector2 srcPos = new Vector2(leaderPos.X, leaderPos.Z);
                Vector2 newPos = srcPos + Geometry.GetRotate(new Vector2(X, Z), leaderDir);
                dir = leaderDir + Dir;
                return new Vector3(newPos.X, leaderPos.Y + Y, newPos.Y);
            }

            public static PosDir Zero = new PosDir();
        }

        public PosDir GetPosDir(int index)
        {
            int ct = m_FormationInfo.Count;
            if (index >= 0 && index < ct) {
                return m_FormationInfo[index];
            } else {
                return PosDir.Zero;
            }
        }
        public void BuildFormationInfo()
        {
            m_FormationInfo.Clear();
            m_FormationInfo.Add(new PosDir(pos0, dir0));
            m_FormationInfo.Add(new PosDir(pos1, dir1));
            m_FormationInfo.Add(new PosDir(pos2, dir2));
            m_FormationInfo.Add(new PosDir(pos3, dir3));
            m_FormationInfo.Add(new PosDir(pos4, dir4));
            m_FormationInfo.Add(new PosDir(pos5, dir5));
            m_FormationInfo.Add(new PosDir(pos6, dir6));
            m_FormationInfo.Add(new PosDir(pos7, dir7));
            m_FormationInfo.Add(new PosDir(pos8, dir8));
            m_FormationInfo.Add(new PosDir(pos9, dir9));
            m_FormationInfo.Add(new PosDir(pos10, dir10));
            m_FormationInfo.Add(new PosDir(pos11, dir11));
            m_FormationInfo.Add(new PosDir(pos12, dir12));
            m_FormationInfo.Add(new PosDir(pos13, dir13));
            m_FormationInfo.Add(new PosDir(pos14, dir14));
            m_FormationInfo.Add(new PosDir(pos15, dir15));
        }

        private List<PosDir> m_FormationInfo = new List<PosDir>();
    }
}
