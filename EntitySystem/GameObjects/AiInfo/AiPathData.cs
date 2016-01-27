using System;
using System.Collections.Generic;
using System.Text;
using ScriptRuntime;

namespace GameFramework
{
  public class AiPathData
  {
    public bool HavePathPoint
    {
      get
      {
        return m_PathPoints.Count > 0;
      }
    }
    public Vector3 CurPathPoint
    {
      get
      {
        Vector3 pos;
        if (m_PathPoints.Count > 0)
          pos = m_PathPoints.Peek();
        else
          pos = new Vector3();
        return pos;
      }
    }
    public Vector3 PathEndPoint
    {
      get
      {
        Vector3 pos = Vector3.Zero;
        if (m_Path.Count > 0) {
          pos = m_Path[m_Path.Count - 1];
        }
        return pos;
      }
    }
    public bool IsReached(Vector3 curPos)
    {
      bool ret = false;
      if (m_PathPoints.Count > 0) {
        Vector3 targetPos = m_PathPoints.Peek();
        float powDistDest = Geometry.DistanceSquare(curPos, targetPos);
        if (powDistDest <= 1f) {
          ret = true;
        }
      }
      return ret;
    }
    public void UseNextPathPoint()
    {
      m_LastPos = CurPathPoint;
      if (m_PathPoints.Count > 0)
        m_PathPoints.Dequeue();
    }
    public void SetPathPoints(Vector3 startPos, IList<Vector3> pts)
    {
      SetPathPoints(startPos, pts, 0);
    }
    public void SetPathPoints(Vector3 startPos, IList<Vector3> pts, int start)
    {
      SetPathPoints(startPos, pts, start, pts.Count - start);
    }
    public void SetPathPoints(Vector3 startPos, IList<Vector3> pts, int start, int len)
    {
      m_PathPoints.Clear();
      m_LastPos = startPos;
      for (int ix = start; ix < pts.Count && ix < start + len; ++ix) {
        m_PathPoints.Enqueue(pts[ix]);
      }
      m_StartPos = startPos;
      m_Path = pts;
      m_PathStart = start;
      m_PathLength = len;
    }
    public void Restart()
    {
      m_PathPoints.Clear();
      m_LastPos = m_StartPos;
      for (int ix = m_PathStart; ix < m_Path.Count && ix < m_PathStart + m_PathLength; ++ix) {
        m_PathPoints.Enqueue(m_Path[ix]);
      }
    }
    public void Clear()
    {
      m_PathPoints.Clear();
      m_Path = null;
      m_PathStart = 0;
      m_PathLength = 0;
    }
    public bool GetNearstPoint(Vector3 pt, ref Vector3 tp)
    {
      bool ret = false;
      if (null != m_Path && m_PathStart >= 0 && m_PathStart + m_PathLength >= 1 && m_PathStart + m_PathLength <= m_Path.Count) {
        if (m_Path.Count > 0) {
          float dist = Geometry.PointToPolylineDistance(pt, m_Path, m_PathStart, m_PathLength, ref tp);
          float r = Geometry.Relation(pt, m_StartPos, m_Path[0]);
          if (r >= 0 && r <= 1) {
            Vector3 t = Geometry.Perpendicular(pt, m_StartPos, m_Path[0]);
            if (Geometry.DistanceSquare(pt, t) < dist * dist)
              tp = t;
            ret = true;
          } else if (dist < 1000) {
            ret = true;
          }
        }
      }
      return ret;
    }
    public long UpdateTime
    {
      get { return m_UpdateTime; }
      set { m_UpdateTime = value; }
    }
    public Vector3 LastPos
    {
      get { return m_LastPos; }
      set { m_LastPos = value; }
    }

    private Queue<Vector3> m_PathPoints = new Queue<Vector3>();
    private Vector3 m_LastPos = new Vector3();
    private Vector3 m_StartPos = new Vector3();
    private IList<Vector3> m_Path = null;
    private int m_PathStart = 0;
    private int m_PathLength = 0;
    private long m_UpdateTime = 0;
  }
}
