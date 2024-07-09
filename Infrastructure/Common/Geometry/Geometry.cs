using System;
using System.Collections.Generic;
using System.Text;
using ScriptRuntime;
using ScriptableFramework;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ScriptableFramework
{
    public partial class Geometry
    {
        public const float c_MinDistance = 0.0001f;
        public const float c_FloatPrecision = 0.0001f;
        public const float c_DoublePrecision = 0.0000001f;
        public static float RadianToDegree(float dir)
        {
            return (float)(dir * 180 / Math.PI);
        }
        public static float DegreeToRadian(float dir)
        {
            return (float)(dir * Math.PI / 180);
        }
        public static bool IsInvalid(float v)
        {
            return float.IsNaN(v) || float.IsInfinity(v);
        }
        public static bool IsInvalid(Vector2 pos)
        {
            return IsInvalid(pos.X) || IsInvalid(pos.Y);
        }
        public static bool IsInvalid(Vector3 pos)
        {
            return IsInvalid(pos.X) || IsInvalid(pos.Y) || IsInvalid(pos.Z);
        }
        public static float Max(float a, float b)
        {
            return (a > b ? a : b);
        }
        public static float Min(float a, float b)
        {
            return (a < b ? a : b);
        }
        public static float DistanceSquare(float x1, float y1, float x2, float y2)
        {
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }
        public static float DistanceSquare(Vector2 p1, Vector2 p2)
        {
            return (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
        }
        public static float Distance(Vector2 p1, Vector2 p2)
        {
            return (float)Math.Sqrt(DistanceSquare(p1, p2));
        }
        public static float DistanceSquare(Vector3 p1, Vector3 p2)
        {
            return (p1.X - p2.X) * (p1.X - p2.X) + (p1.Z - p2.Z) * (p1.Z - p2.Z);
        }
        public static float Distance(Vector3 p1, Vector3 p2)
        {
            return (float)Math.Sqrt(DistanceSquare(p1, p2));
        }
        public static float CalcLength(IList<Vector2> pts)
        {
            float len = 0;
            for (int i = 0; i < pts.Count - 1; ++i) {
                len += Distance(pts[i], pts[i + 1]);
            }
            return len;
        }
        public static float CalcLength(IList<Vector3> pts)
        {
            float len = 0;
            for (int i = 0; i < pts.Count - 1; ++i) {
                len += Distance(pts[i], pts[i + 1]);
            }
            return len;
        }
        public static bool IsSameFloat(float v1, float v2)
        {
            return Math.Abs(v1 - v2) < c_FloatPrecision;
        }
        public static bool IsSameDouble(double v1, double v2)
        {
            return Math.Abs(v1 - v2) < c_DoublePrecision;
        }
        public static bool IsSamePoint(Vector2 p1, Vector2 p2)
        {
            return IsSameFloat(p1.X, p2.X) && IsSameFloat(p1.Y, p2.Y);
        }
        public static bool IsSamePoint(Vector3 p1, Vector3 p2)
        {
            return IsSameFloat(p1.X, p2.X) && IsSameFloat(p1.Z, p2.Z);
        }
        public static Vector2 GetBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            t = MathHelper.Clamp(t, 0, 1);
            float num = 1f - t;
            return (Vector2)((((num * num) * p0) + (((2f * num) * t) * p1)) + ((t * t) * p2));
        }
        public static Vector2 GetBezierPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            t = MathHelper.Clamp(t, 0, 1);
            float num = 1f - t;
            return (Vector2)(num * num * num * p0 + 3 * num * num * t * p1 + 3 * num * t * t * p2 + ((t * t * t) * p3));
        }
        public static Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            t = MathHelper.Clamp(t, 0, 1);
            float num = 1f - t;
            return (Vector3)((((num * num) * p0) + (((2f * num) * t) * p1)) + ((t * t) * p2));
        }
        public static Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = MathHelper.Clamp(t, 0, 1);
            float num = 1f - t;
            return (Vector3)(num * num * num * p0 + 3 * num * num * t * p1 + 3 * num * t * t * p2 + ((t * t * t) * p3));
        }
        public static Vector2 FrontOfTarget(Vector2 from, Vector2 to, float mHitDistance)
        {
            Vector2 dir = from - to;
            dir.Normalize();
            Vector2 value = to + dir * mHitDistance;
            return value;
        }
        public static Vector3 FrontOfTarget(Vector3 from, Vector3 to, float mHitDistance)
        {
            Vector3 dir = from - to;
            dir.Normalize();
            Vector3 value = to + dir * mHitDistance;
            return value;
        }
    }
    public partial class Geometry
    {
        #region miloyip "A Brief Analysis of Sector and Disk Intersection Test"
        //-------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------The following functions come from miloyip's "A Brief Analysis of Sector and Disk Intersection Tests"----------------------------------------
        /// <summary>
        /// Calculate the shortest square distance between a line segment and a point
        /// x0 starting point of line segment
        /// u  Line segment direction to end point
        /// x  any point
        /// </summary>
        /// <returns></returns>
        public static float SegmentPointSqrDistance(Vector2 x0, Vector2 u, Vector2 x)
        {
            float t = Vector2.Dot(x - x0, u) / u.LengthSquared();
            return (x - (x0 + MathHelper.Clamp(t, 0, 1) * u)).LengthSquared();
        }
        /// <summary>
        /// x0 Starting point of capsule line segment
        /// u  Capsule line segment direction to end point
        /// cr capsule radius
        /// c Disk center
        /// r Disk radius
        /// </summary>
        /// <returns></returns>
        public static bool IsCapsuleDiskIntersect(Vector2 x0, Vector2 u, float cr, Vector2 c, float r)
        {
            return SegmentPointSqrDistance(x0, u, c) <= (cr + r) * (cr + r);
        }
        /// <summary>
        /// c AABB Center
        /// h AABB half length
        /// p center of disc
        /// r radius of disk
        /// </summary>
        public static bool IsAabbDiskIntersect(Vector2 c, Vector2 h, Vector2 p, float r) {
            Vector2 v = Vector2.Max(p - c, c - p); // = Abs(p - c);
            Vector2 u = Vector2.Max(v - h, Vector2.Zero);
            return u.LengthSquared() <= r * r;
        }
        /// <summary>
        /// Intersection of circle and rectangle
        /// </summary>
        /// <param name="c">center of rectangle</param>
        /// <param name="h">Rectangle half length</param>
        /// <param name="angle">Rectangular direction</param>
        /// <param name="p">center of circle</param>
        /// <param name="r">circle radius</param>
        /// <returns></returns>
        public static bool IsObbDiskIntersect(Vector2 c, Vector2 h, float angle, Vector2 p, float r)
        {
            Vector2 nc = GetRotate(c, -angle);
            Vector2 np = GetRotate(p, -angle);
            return IsAabbDiskIntersect(nc, h, np, r);
        }
        ///<summary>
        /// Sector and disk intersection test
        /// a Sector center
        /// u Sector direction (unit vector)
        /// theta Sector Sweep Half Angle
        /// l Sector side length
        /// c Disk center
        /// r Disk radius
        ///</summary>
        public static bool IsSectorDiskIntersect(Vector2 a, Vector2 u, float theta, float l, Vector2 c, float r)
        {
            //UnityEngine.LogSystem.Warn("IsSectorDiskIntersect,u:" + u);
            //UnityEngine.LogSystem.Warn("a:" + a + ",u:" + u + ",theta:" + theta + ",l" + l + ",c" + c + ",r:" + r);
            // 1. If the directions of the center of the sector and the center of the disk can be separated, the two shapes will not intersect.
            Vector2 d = c - a;
            float rsum = l + r;
            if (d.LengthSquared() > rsum * rsum)
                return false;

            // 2. Calculate p of the sector local space
            float px = Vector2.Dot(d, u);
            float py = MathHelper.Abs(Vector2.Dot(d, new Vector2(-u.Y, u.X)));

            // 3. If p_x > ||p|| cos theta, the two shapes intersect
            if (px > d.Length() * MathHelper.Cos(theta))
                return true;

            // 4. Find whether the left line segment intersects the disk
            Vector2 q = l * new Vector2(MathHelper.Cos(theta), MathHelper.Sin(theta));
            Vector2 p = new Vector2(px, py);
            return SegmentPointSqrDistance(Vector2.Zero, q, p) <= r * r;
        }
        //-------------------------------------------------------------------------------------------------------------------------------
        #endregion
    }
    //The coordinate system is a left-handed coordinate system.
    //The angle of 0 degrees is the positive direction of the z-axis.
    //The rotation direction is the positive direction of the z-axis and the positive
    //direction of the x-axis
    //(counterclockwise rotation when viewed along the positive y-axis).
    public partial class Geometry
    {
        public static float GetYRadian(Vector2 fvPos1, Vector2 fvPos2)
        {
            float dDistance = (float)Vector2.Distance(fvPos1, fvPos2);
            if (dDistance <= 0.0f)
                return 0.0f;

            float fACos = (fvPos2.Y - fvPos1.Y) / dDistance;
            if (fACos > 1.0)
                fACos = 0.0f;
            else if (fACos < -1.0)
                fACos = (float)Math.PI;
            else
                fACos = (float)Math.Acos(fACos);

            // [0~180]
            if (fvPos2.X >= fvPos1.X)
                return (float)fACos;
            //(180, 360)
            else
                return (float)(Math.PI * 2 - fACos);
        }
        public static Vector2 GetRotate(Vector2 fvPos1, float radian)
        {
            radian = -radian;
            // pos1 -> pos2
            Vector2 retV = new Vector2(
                (float)(fvPos1.X * Math.Cos(radian) - fvPos1.Y * Math.Sin(radian)),
                (float)(fvPos1.X * Math.Sin(radian) + fvPos1.Y * Math.Cos(radian))
                );
            return retV;
        }
        public static float GetYRadian(ScriptRuntime.Vector3 pos1, ScriptRuntime.Vector3 pos2)
        {
            return GetYRadian(new Vector2(pos1.X, pos1.Z), new Vector2(pos2.X, pos2.Z));
        }
        public static Vector3 GetRotate(Vector3 pos, float radian)
        {
            Vector2 newPos = GetRotate(new Vector2(pos.X, pos.Z), radian);
            return new Vector3(newPos.X, 0, newPos.Y);
        }
        public static Vector3 TransformPoint(Vector3 pos, Vector3 offset, float radian)
        {
            Vector3 newOffset = Geometry.GetRotate(offset, radian);
            newOffset.Y = offset.Y;
            Vector3 result = pos + newOffset;
            return result;
        }
    }
    public partial class Geometry
    {
        /// <summary>
        /// Compute vector cross product
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="ep"></param>
        /// <param name="op"></param>
        /// <returns>
        /// ret>0 ep in opsp vector counterclockwise direction
        /// ret=0 collinear
        /// ret<0 ep in opsp vector clockwise direction
        /// </returns>
        public static float Multiply(Vector2 sp, Vector2 ep, Vector2 op)
        {
            return ((sp.X - op.X) * (ep.Y - op.Y) - (ep.X - op.X) * (sp.Y - op.Y));
        }
        /// <summary>
        /// r=DotMultiply(p1,p2,p0), get the dot product of vectors (p1-p0) and (p2-p0).
        /// Note: Both vectors must be non-zero vectors
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p0"></param>
        /// <returns>
        /// r>0:The angle between two vectors is an acute angle;
        /// r=0:The angle between the two vectors is a right angle;
        /// r<0:The angle between two vectors is an obtuse angle
        /// </returns>
        public static float DotMultiply(Vector2 p1, Vector2 p2, Vector2 p0)
        {
            return ((p1.X - p0.X) * (p2.X - p0.X) + (p1.Y - p0.Y) * (p2.Y - p0.Y));
        }
        /// <summary>
        /// Determine the relationship between p and line segment p1p2
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns>
        /// r=0 p = p1
        /// r=1 p = p2
        /// r<0 p is on the backward extension of p1p2
        /// r>1 p is on the forward extension of p1p2
        /// 0<r<1 p is interior to p1p2
        /// </returns>
        public static float Relation(Vector2 p, Vector2 p1, Vector2 p2)
        {
            return DotMultiply(p, p2, p1) / DistanceSquare(p1, p2);
        }
        /// <summary>
        /// Ask for a foothold
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Vector2 Perpendicular(Vector2 p, Vector2 p1, Vector2 p2)
        {
            float r = Relation(p, p1, p2);
            Vector2 tp = new Vector2();
            tp.X = p1.X + r * (p2.X - p1.X);
            tp.Y = p1.Y + r * (p2.Y - p1.Y);
            return tp;
        }
        /// <summary>
        /// Finds the minimum distance from a point to a line segment and
        /// returns the closest point.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="np"></param>
        /// <returns></returns>
        public static float PointToLineSegmentDistance(Vector2 p, Vector2 p1, Vector2 p2, out Vector2 np)
        {
            float r = Relation(p, p1, p2);
            if (r < 0) {
                np = p1;
                return Distance(p, p1);
            }
            if (r > 1) {
                np = p2;
                return Distance(p, p2);
            }
            np = Perpendicular(p, p1, p2);
            return Distance(p, np);
        }
        public static float PointToLineSegmentDistanceSquare(Vector2 p, Vector2 p1, Vector2 p2, out Vector2 np)
        {
            float r = Relation(p, p1, p2);
            if (r < 0) {
                np = p1;
                return DistanceSquare(p, p1);
            }
            if (r > 1) {
                np = p2;
                return DistanceSquare(p, p2);
            }
            np = Perpendicular(p, p1, p2);
            return DistanceSquare(p, np);
        }
        /// <summary>
        /// Find the distance from a point to a straight line
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float PointToLineDistance(Vector2 p, Vector2 p1, Vector2 p2)
        {
            return Math.Abs(Multiply(p, p2, p1)) / Distance(p1, p2);
        }
        public static float PointToLineDistanceInverse(Vector2 p, Vector2 p1, Vector2 p2)
        {
            return Math.Abs(Multiply(p, p2, p1)) / Distance(p1, p2);
        }
        /// <summary>
        /// Find the minimum distance from the point to the polyline and return the minimum distance point.
        /// Note: If the given point is not enough to form a polyline, the value of q will not be modified.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pts"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static float PointToPolylineDistance(Vector2 p, IList<Vector2> pts, int start, int len, ref Vector2 q)
        {
            float ret = float.MaxValue;
            for (int i = start; i < start + len - 1; ++i) {
                Vector2 tq;
                float d = PointToLineSegmentDistance(p, pts[i], pts[i + 1], out tq);
                if (d < ret) {
                    ret = d;
                    q = tq;
                }
            }
            return ret;
        }
        public static float PointToPolylineDistanceSquare(Vector2 p, IList<Vector2> pts, int start, int len, ref Vector2 q)
        {
            float ret = float.MaxValue;
            for (int i = start; i < start + len - 1; ++i) {
                Vector2 tq;
                float d = PointToLineSegmentDistanceSquare(p, pts[i], pts[i + 1], out tq);
                if (d < ret) {
                    ret = d;
                    q = tq;
                }
            }
            return ret;
        }
        public static bool Intersect(Vector2 us, Vector2 ue, Vector2 vs, Vector2 ve)
        {
            return ((Max(us.X, ue.X) >= Min(vs.X, ve.X)) &&
            (Max(vs.X, ve.X) >= Min(us.X, ue.X)) &&
            (Max(us.Y, ue.Y) >= Min(vs.Y, ve.Y)) &&
            (Max(vs.Y, ve.Y) >= Min(us.Y, ue.Y)) &&
            (Multiply(vs, ue, us) * Multiply(ue, ve, us) >= 0) &&
            (Multiply(us, ve, vs) * Multiply(ve, ue, vs) >= 0));
        }
        public static bool LineIntersectRectangle(Vector2 lines, Vector2 linee, float left, float top, float right, float bottom)
        {
            if (Max(lines.X, linee.X) >= left && right >= Min(lines.X, linee.X) && Max(lines.Y, linee.Y) >= top && bottom >= Min(lines.Y, linee.Y)) {
                Vector2 lt = new Vector2(left, top);
                Vector2 rt = new Vector2(right, top);
                Vector2 rb = new Vector2(right, bottom);
                Vector2 lb = new Vector2(left, bottom);
                if (Multiply(lines, lt, linee) * Multiply(lines, rt, linee) <= 0)
                    return true;
                if (Multiply(lines, lt, linee) * Multiply(lines, lb, linee) <= 0)
                    return true;
                if (Multiply(lines, rt, linee) * Multiply(lines, rb, linee) <= 0)
                    return true;
                if (Multiply(lines, lb, linee) * Multiply(lines, rb, linee) <= 0)
                    return true;
                /*
                if (Intersect(lines, linee, lt, rt))
                  return true;
                if (Intersect(lines, linee, rt, rb))
                  return true;
                if (Intersect(lines, linee, rb, lb))
                  return true;
                if (Intersect(lines, linee, lt, lb))
                  return true;
                */
            }
            return false;
        }
        public static bool PointOnLine(Vector2 pt, Vector2 pt1, Vector2 pt2)
        {
            float dx, dy, dx1, dy1;
            bool retVal;
            dx = pt2.X - pt1.X;
            dy = pt2.Y - pt1.Y;
            dx1 = pt.X - pt1.X;
            dy1 = pt.Y - pt1.Y;
            if (pt.X == pt1.X && pt.Y == pt1.Y || pt.X == pt2.X && pt.Y == pt2.Y) {//on the vertex
                retVal = true;
            } else {
                if (Math.Abs(dx * dy1 - dy * dx1) < c_FloatPrecision) {//Slopes are equal//Consider calculation errors
                    //equals zero at the vertex
                    if (dx1 * (dx1 - dx) < 0 | dy1 * (dy1 - dy) < 0) {
                        retVal = true;
                    } else {
                        retVal = false;
                    }
                } else {
                    retVal = false;
                }
            }
            return retVal;
        }
        /// <summary>
        /// Determine whether two points coincide with each other
        /// </summary>
        /// <param name="pt0"></param>
        /// <param name="pt1"></param>
        /// <returns></returns>
        public static bool PointOverlapPoint(Vector2 pt0, Vector2 pt1)
        {
            if (Distance(pt0, pt1) < c_MinDistance)
                return true;
            else
                return false;
        }
        public static bool RectangleOverlapRectangle(float _x1, float _y1, float _x2, float _y2, float x1, float y1, float x2, float y2)
        {
            if (_x1 > x2 + c_FloatPrecision)
                return false;
            if (_x2 < x1 - c_FloatPrecision)
                return false;
            if (_y1 > y2 + c_FloatPrecision)
                return false;
            if (_y2 < y1 - c_FloatPrecision)
                return false;
            return true;
        }
        /// <summary>
        /// Determine whether a is to the left of line bc
        /// </summary>
        public static bool PointIsLeft(Vector2 a, Vector2 b, Vector2 c)
        {
            return Multiply(c, a, b) > 0;
        }
        /// <summary>
        /// Determine whether a is to the left or on the line bc
        /// </summary>
        public static bool PointIsLeftOn(Vector2 a, Vector2 b, Vector2 c)
        {
            return Multiply(c, a, b) >= 0;
        }
        /// <summary>
        /// Determine whether the three points a, b, and c are collinear
        /// </summary>
        public static bool PointIsCollinear(Vector2 a, Vector2 b, Vector2 c)
        {
            return Multiply(c, a, b) == 0;
        }
        public static bool IsCounterClockwise(IList<Vector2> pts, int start, int len)
        {
            bool ret = false;
            if (len >= 3) {
                float maxx = pts[start].X;
                float maxy = pts[start].Y;
                float minx = maxx;
                float miny = maxy;
                int maxxi = 0;
                int maxyi = 0;
                int minxi = 0;
                int minyi = 0;
                for (int i = 1; i < len; ++i) {
                    int ix = start + i;
                    float x = pts[ix].X;
                    float y = pts[ix].Y;
                    if (maxx < x) {
                        maxx = x;
                        maxxi = i;
                    } else if (minx > x) {
                        minx = x;
                        minxi = i;
                    }
                    if (maxy < y) {
                        maxy = y;
                        maxyi = i;
                    } else if (miny > y) {
                        miny = y;
                        minyi = i;
                    }
                }
                int val = 0;
                for (int delt = 0; delt < len; delt++) {
                    int ix0 = start + (len * 2 + maxxi - delt - 1) % len;
                    int ix1 = start + maxxi;
                    int ix2 = start + (maxxi + delt + 1) % len;

                    float r = Multiply(pts[ix1], pts[ix2], pts[ix0]);
                    if (r > 0) {
                        val++;
                        break;
                    } else if (r < 0) {
                        val--;
                        break;
                    }
                }
                for (int delt = 0; delt < len; delt++) {
                    int ix0 = start + (len * 2 + maxyi - delt - 1) % len;
                    int ix1 = start + maxyi;
                    int ix2 = start + (maxyi + delt + 1) % len;

                    float r = Multiply(pts[ix1], pts[ix2], pts[ix0]);
                    if (r > 0) {
                        val++;
                        break;
                    } else if (r < 0) {
                        val--;
                        break;
                    }
                }
                for (int delt = 0; delt < len; delt++) {
                    int ix0 = start + (len * 2 + minxi - delt - 1) % len;
                    int ix1 = start + minxi;
                    int ix2 = start + (minxi + delt + 1) % len;

                    float r = Multiply(pts[ix1], pts[ix2], pts[ix0]);
                    if (r > 0) {
                        val++;
                        break;
                    } else if (r < 0) {
                        val--;
                        break;
                    }
                }
                for (int delt = 0; delt < len; delt++) {
                    int ix0 = start + (len * 2 + minyi - delt - 1) % len;
                    int ix1 = start + minyi;
                    int ix2 = start + (minyi + delt + 1) % len;

                    float r = Multiply(pts[ix1], pts[ix2], pts[ix0]);
                    if (r > 0) {
                        val++;
                        break;
                    } else if (r < 0) {
                        val--;
                        break;
                    }
                }
                if (val > 0)
                    ret = true;
                else
                    ret = false;
            }
            return ret;
        }
        /// <summary>
        /// Calculate polygon centroid and radius
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <param name="centroid"></param>
        /// <returns></returns>
        public static float CalcPolygonCentroidAndRadius(IList<Vector2> pts, int start, int len, out Vector2 centroid)
        {
            float ret = 0;
            if (len > 0) {
                float x = 0;
                float y = 0;
                for (int i = 1; i < len; ++i) {
                    int ix = start + i;
                    x += pts[ix].X;
                    y += pts[ix].Y;
                }
                x /= len;
                y /= len;
                centroid = new Vector2(x, y);
                float distSqr = 0;
                for (int i = 1; i < len; ++i) {
                    int ix = start + i;
                    float tmp = Geometry.DistanceSquare(centroid, pts[ix]);
                    if (distSqr < tmp)
                        distSqr = tmp;
                }
                ret = (float)Math.Sqrt(distSqr);
            } else {
                centroid = new Vector2();
            }
            return ret;
        }
        /// <summary>
        /// Compute polygonal axis-aligned rectangular bounding box
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <param name="minXval"></param>
        /// <param name="maxXval"></param>
        /// <param name="minZval"></param>
        /// <param name="maxZval"></param>
        public static void CalcPolygonBound(IList<Vector2> pts, int start, int len, out float minXval, out float maxXval, out float minYval, out float maxYval)
        {
            maxXval = pts[start].X;
            minXval = pts[start].X;
            maxYval = pts[start].Y;
            minYval = pts[start].Y;
            for (int i = 1; i < len; ++i) {
                int ix = start + i;
                float xv = pts[ix].X;
                float yv = pts[ix].Y;
                if (maxXval < xv)
                    maxXval = xv;
                else if (minXval > xv)
                    minXval = xv;
                if (maxYval < yv)
                    maxYval = yv;
                else if (minYval > yv)
                    minYval = yv;
            }
        }
        /// <summary>
        /// Determine whether a point is within a polygon
        /// </summary>
        /// <typeparam name="PointT"></typeparam>
        /// <param name="pt"></param>
        /// <param name="pts"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns>
        /// 1  -- within polygon
        /// 0  -- on polygon edge
        /// -1 -- outside the polygon 
        /// </returns>
        public static int PointInPolygon(Vector2 pt, IList<Vector2> pts, int start, int len)
        {
            float maxXval;
            float minXval;
            float maxYval;
            float minYval;
            CalcPolygonBound(pts, start, len, out minXval, out maxXval, out minYval, out maxYval);
            return PointInPolygon(pt, pts, start, len, minXval, maxXval, minYval, maxYval);
        }

        public static int PointInPolygon(Vector2 pt, IList<Vector2> pts, int start, int len, float minXval, float maxXval, float minYval, float maxYval)
        {
            if ((pt.X > maxXval) | (pt.X < minXval) | pt.Y > maxYval | pt.Y < minYval) {
                return -1;//outside the polygon
            } else {
                int cnt = 0;
                int lastStatus;
                Vector2 pt0 = pts[start];
                if (pt0.Y - pt.Y > c_FloatPrecision)
                    lastStatus = 1;
                else if (pt0.Y - pt.Y < c_FloatPrecision)
                    lastStatus = -1;
                else
                    lastStatus = 0;

                for (int i = 0; i < len; ++i) {
                    int ix1 = start + i;
                    int ix2 = start + (i + 1) % len;
                    Vector2 pt1 = pts[ix1];
                    Vector2 pt2 = pts[ix2];

                    if (PointOnLine(pt, pt1, pt2))
                        return 0;

                    int status;
                    if (pt2.Y - pt.Y > c_FloatPrecision)
                        status = 1;
                    else if (pt2.Y - pt.Y < c_FloatPrecision)
                        status = -1;
                    else
                        status = 0;

                    int temp = status - lastStatus;
                    lastStatus = status;
                    if (temp > 0) {
                        if (!PointIsLeftOn(pt, pt1, pt2))
                            cnt += temp;
                    } else if (temp < 0) {
                        if (PointIsLeft(pt, pt1, pt2))
                            cnt += temp;
                    }
                }
                if (cnt == 0)
                    return -1;
                else
                    return 1;
            }
        }

        /// <summary>
        /// Determine the relationship between a point and the triangle formed by three other points
        /// Note: The three points A B C of the triangle must be arranged counterclockwise
        /// -1-outside the triangle 0-inside the triangle 1-on the triangle side ab 2-on the triangle side bc 3-on the triangle side ca 4-on the triangle vertex/// </summary>
        /// <param name="pn">point</param>
        /// <param name="pa">triangle vertex A</param>
        /// <param name="pb">triangle vertex B</param>
        /// <param name="pc">triangle vertex C</param>
        /// <returns>-1-Outside the triangle 0-Inside the triangle 1-On the triangle side ab 2-On the triangle side bc 3-On the triangle side ca 4-On the triangle vertex</returns>
        public static int PointInTriangle(Vector2 pn, Vector2 pa, Vector2 pb, Vector2 pc)
        {
            //First, determine whether it is on the edge
            //Note: The three points A B C of the triangle must be arranged counterclockwise
            int num = 0;
            int side = 0;
            if (PointOnLine(pn, pa, pb)) {
                side = 1;
                num++;
            }
            if (PointOnLine(pn, pb, pc)) {
                side = 2;
                num++;
            }
            if (PointOnLine(pn, pc, pa)) {
                side = 3;
                num++;
            }
            if (num > 1)
                return 4;
            else if (num == 1)
                return side;
            else {
                if (SignArea(pn, pa, pb) >= 0.0f && SignArea(pn, pb, pc) >= 0.0f && SignArea(pn, pc, pa) >= 0.0f)
                    return 0;
                else
                    return -1;
            }
        }
        /// <summary>
        /// To calculate the signed area of a triangle, enter the coordinates of the three vertices of the triangle.
        /// </summary>
        /// <param name="na"></param>
        /// <param name="nb"></param>
        /// <param name="nc"></param>
        /// <returns></returns>
        public static float SignArea(Vector2 na, Vector2 nb, Vector2 nc)
        {
            return ((nb.X - na.X) * (nc.Y - na.Y) - (nb.Y - na.Y) * (nc.X - na.X)) / 2;
        }
        /// <summary>
        /// Thin out a polygon or polyline.
        /// </summary>
        /// <typeparam name="PointT"></typeparam>
        /// <param name="pts"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <param name="delta">Distance to determine if three specified points are collinear</param>
        /// <returns></returns>
        public static IList<Vector2> Sparseness(IList<Vector2> pts, int start, int len, float delta)
        {
            Vector2 lastPt = pts[0];
            LinkedList<Vector2> links = new LinkedList<Vector2>();
            links.AddLast(lastPt);
            for (int i = 1; i < len; i++) {
                int ix = start + i;
                Vector2 pt = pts[ix];
                if (!PointOverlapPoint(pt, lastPt)) {
                    links.AddLast(pt);
                }
                lastPt = pt;
            }
            for (int count = 1; count > 0; ) {
                count = 0;
                LinkedListNode<Vector2> node = links.First;
                while (node != null) {
                    LinkedListNode<Vector2> node2 = node.Next;
                    LinkedListNode<Vector2> node3 = null;
                    if (node2 != null)
                        node3 = node2.Next;
                    if (node3 != null) {
                        double d = PointToLineDistance(node2.Value, node.Value, node3.Value);
                        if (d <= delta) {
                            links.Remove(node2);
                            count++;
                        }
                    }
                    //Regardless of whether to delete a node, always remove the next node, which can avoid some situations where arcs are drawn into straight lines.
                    node = node.Next;
                }
            }
            Vector2[] rpts = new Vector2[links.Count];
            links.CopyTo(rpts, 0);
            return rpts;
        }
    }
    public partial class Geometry
    {
        /// <summary>
        /// Compute vector cross product
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="ep"></param>
        /// <param name="op"></param>
        /// <returns>
        /// ret>0 ep in opsp vector counterclockwise direction
        /// ret=0 collinear
        /// ret<0 ep in opsp vector clockwise direction
        /// </returns>
        public static float Multiply(Vector3 sp, Vector3 ep, Vector3 op)
        {
            return ((sp.X - op.X) * (ep.Z - op.Z) - (ep.X - op.X) * (sp.Z - op.Z));
        }
        /// <summary>
        /// r=DotMultiply(p1,p2,p0), get the dot product of vectors (p1-p0) and (p2-p0).
        /// Note: Both vectors must be non-zero vectors
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p0"></param>
        /// <returns>
        /// r>0: The angle between the two vectors is an acute angle;
        /// r=0: The angle between the two vectors is a right angle;
        /// r<0: The angle between the two vectors is an obtuse angle
        /// </returns>
        public static float DotMultiply(Vector3 p1, Vector3 p2, Vector3 p0)
        {
            return ((p1.X - p0.X) * (p2.X - p0.X) + (p1.Z - p0.Z) * (p2.Z - p0.Z));
        }
        /// <summary>
        /// Determine the relationship between p and line segment p1p2
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns>
        /// r=0 p = p1
        /// r=1 p = p2
        /// r<0 p is on the backward extension of p1p2
        /// r>1 p is on the forward extension of p1p2
        /// 0<r<1 p is interior to p1p2
        /// </returns>
        public static float Relation(Vector3 p, Vector3 p1, Vector3 p2)
        {
            return DotMultiply(p, p2, p1) / DistanceSquare(p1, p2);
        }
        /// <summary>
        /// Ask for a foothold
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Vector3 Perpendicular(Vector3 p, Vector3 p1, Vector3 p2)
        {
            float r = Relation(p, p1, p2);
            Vector3 tp = new Vector3();
            tp.X = p1.X + r * (p2.X - p1.X);
            tp.Z = p1.Z + r * (p2.Z - p1.Z);
            return tp;
        }
        /// <summary>
        /// Finds the minimum distance from a point to a line segment and returns the closest point.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="np"></param>
        /// <returns></returns>
        public static float PointToLineSegmentDistance(Vector3 p, Vector3 p1, Vector3 p2, out Vector3 np)
        {
            float r = Relation(p, p1, p2);
            if (r < 0) {
                np = p1;
                return Distance(p, p1);
            }
            if (r > 1) {
                np = p2;
                return Distance(p, p2);
            }
            np = Perpendicular(p, p1, p2);
            return Distance(p, np);
        }
        public static float PointToLineSegmentDistanceSquare(Vector3 p, Vector3 p1, Vector3 p2, out Vector3 np)
        {
            float r = Relation(p, p1, p2);
            if (r < 0) {
                np = p1;
                return DistanceSquare(p, p1);
            }
            if (r > 1) {
                np = p2;
                return DistanceSquare(p, p2);
            }
            np = Perpendicular(p, p1, p2);
            return DistanceSquare(p, np);
        }
        /// <summary>
        /// Find the distance from a point to a straight line
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float PointToLineDistance(Vector3 p, Vector3 p1, Vector3 p2)
        {
            return Math.Abs(Multiply(p, p2, p1)) / Distance(p1, p2);
        }
        public static float PointToLineDistanceInverse(Vector3 p, Vector3 p1, Vector3 p2)
        {
            return Math.Abs(Multiply(p, p2, p1)) / Distance(p1, p2);
        }
        /// <summary>
        /// Find the minimum distance from the point to the polyline and return the minimum distance point.
        /// Note: If the given point is not enough to form a polyline, the value of q will not be modified.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pts"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static float PointToPolylineDistance(Vector3 p, IList<Vector3> pts, int start, int len, ref Vector3 q)
        {
            float ret = float.MaxValue;
            for (int i = start; i < start + len - 1; ++i) {
                Vector3 tq;
                float d = PointToLineSegmentDistance(p, pts[i], pts[i + 1], out tq);
                if (d < ret) {
                    ret = d;
                    q = tq;
                }
            }
            return ret;
        }
        public static float PointToPolylineDistanceSquare(Vector3 p, IList<Vector3> pts, int start, int len, ref Vector3 q)
        {
            float ret = float.MaxValue;
            for (int i = start; i < start + len - 1; ++i) {
                Vector3 tq;
                float d = PointToLineSegmentDistanceSquare(p, pts[i], pts[i + 1], out tq);
                if (d < ret) {
                    ret = d;
                    q = tq;
                }
            }
            return ret;
        }
        public static bool Intersect(Vector3 us, Vector3 ue, Vector3 vs, Vector3 ve)
        {
            return ((Max(us.X, ue.X) >= Min(vs.X, ve.X)) &&
            (Max(vs.X, ve.X) >= Min(us.X, ue.X)) &&
            (Max(us.Z, ue.Z) >= Min(vs.Z, ve.Z)) &&
            (Max(vs.Z, ve.Z) >= Min(us.Z, ue.Z)) &&
            (Multiply(vs, ue, us) * Multiply(ue, ve, us) >= 0) &&
            (Multiply(us, ve, vs) * Multiply(ve, ue, vs) >= 0));
        }
        public static bool LineIntersectRectangle(Vector3 lines, Vector3 linee, float left, float top, float right, float bottom)
        {
            if (Max(lines.X, linee.X) >= left && right >= Min(lines.X, linee.X) && Max(lines.Z, linee.Z) >= top && bottom >= Min(lines.Z, linee.Z)) {
                Vector3 lt = new Vector3(left, 0, top);
                Vector3 rt = new Vector3(right, 0, top);
                Vector3 rb = new Vector3(right, 0, bottom);
                Vector3 lb = new Vector3(left, 0, bottom);
                if (Multiply(lines, lt, linee) * Multiply(lines, rt, linee) <= 0)
                    return true;
                if (Multiply(lines, lt, linee) * Multiply(lines, lb, linee) <= 0)
                    return true;
                if (Multiply(lines, rt, linee) * Multiply(lines, rb, linee) <= 0)
                    return true;
                if (Multiply(lines, lb, linee) * Multiply(lines, rb, linee) <= 0)
                    return true;
                /*
                if (Intersect(lines, linee, lt, rt))
                  return true;
                if (Intersect(lines, linee, rt, rb))
                  return true;
                if (Intersect(lines, linee, rb, lb))
                  return true;
                if (Intersect(lines, linee, lt, lb))
                  return true;
                */
            }
            return false;
        }
        public static bool PointOnLine(Vector3 pt, Vector3 pt1, Vector3 pt2)
        {
            float dx, dy, dx1, dy1;
            bool retVal;
            dx = pt2.X - pt1.X;
            dy = pt2.Z - pt1.Z;
            dx1 = pt.X - pt1.X;
            dy1 = pt.Z - pt1.Z;
            if (pt.X == pt1.X && pt.Z == pt1.Z || pt.X == pt2.X && pt.Z == pt2.Z) {//on the vertex
                retVal = true;
            } else {
                if (Math.Abs(dx * dy1 - dy * dx1) < c_FloatPrecision) {//Slopes are equal//Consider calculation errors
                    //equals zero at the vertex
                    if (dx1 * (dx1 - dx) < 0 | dy1 * (dy1 - dy) < 0) {
                        retVal = true;
                    } else {
                        retVal = false;
                    }
                } else {
                    retVal = false;
                }
            }
            return retVal;
        }
        /// <summary>
        /// Determine whether two points coincide with each other
        /// </summary>
        /// <param name="pt0"></param>
        /// <param name="pt1"></param>
        /// <returns></returns>
        public static bool PointOverlapPoint(Vector3 pt0, Vector3 pt1)
        {
            if (Distance(pt0, pt1) < c_MinDistance)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Determine whether a is to the left of line bc
        /// </summary>
        public static bool PointIsLeft(Vector3 a, Vector3 b, Vector3 c)
        {
            return Multiply(c, a, b) > 0;
        }
        /// <summary>
        /// Determine whether a is to the left or on the line bc
        /// </summary>
        public static bool PointIsLeftOn(Vector3 a, Vector3 b, Vector3 c)
        {
            return Multiply(c, a, b) >= 0;
        }
        /// <summary>
        /// Determine whether the three points a, b, and c are collinear
        /// </summary>
        public static bool PointIsCollinear(Vector3 a, Vector3 b, Vector3 c)
        {
            return Multiply(c, a, b) == 0;
        }
        public static bool IsCounterClockwise(IList<Vector3> pts, int start, int len)
        {
            bool ret = false;
            if (len >= 3) {
                float maxx = pts[start].X;
                float maxy = pts[start].Z;
                float minx = maxx;
                float miny = maxy;
                int maxxi = 0;
                int maxyi = 0;
                int minxi = 0;
                int minyi = 0;
                for (int i = 1; i < len; ++i) {
                    int ix = start + i;
                    float x = pts[ix].X;
                    float y = pts[ix].Z;
                    if (maxx < x) {
                        maxx = x;
                        maxxi = i;
                    } else if (minx > x) {
                        minx = x;
                        minxi = i;
                    }
                    if (maxy < y) {
                        maxy = y;
                        maxyi = i;
                    } else if (miny > y) {
                        miny = y;
                        minyi = i;
                    }
                }
                int val = 0;
                for (int delt = 0; delt < len; delt++) {
                    int ix0 = start + (len * 2 + maxxi - delt - 1) % len;
                    int ix1 = start + maxxi;
                    int ix2 = start + (maxxi + delt + 1) % len;

                    float r = Multiply(pts[ix1], pts[ix2], pts[ix0]);
                    if (r > 0) {
                        val++;
                        break;
                    } else if (r < 0) {
                        val--;
                        break;
                    }
                }
                for (int delt = 0; delt < len; delt++) {
                    int ix0 = start + (len * 2 + maxyi - delt - 1) % len;
                    int ix1 = start + maxyi;
                    int ix2 = start + (maxyi + delt + 1) % len;

                    float r = Multiply(pts[ix1], pts[ix2], pts[ix0]);
                    if (r > 0) {
                        val++;
                        break;
                    } else if (r < 0) {
                        val--;
                        break;
                    }
                }
                for (int delt = 0; delt < len; delt++) {
                    int ix0 = start + (len * 2 + minxi - delt - 1) % len;
                    int ix1 = start + minxi;
                    int ix2 = start + (minxi + delt + 1) % len;

                    float r = Multiply(pts[ix1], pts[ix2], pts[ix0]);
                    if (r > 0) {
                        val++;
                        break;
                    } else if (r < 0) {
                        val--;
                        break;
                    }
                }
                for (int delt = 0; delt < len; delt++) {
                    int ix0 = start + (len * 2 + minyi - delt - 1) % len;
                    int ix1 = start + minyi;
                    int ix2 = start + (minyi + delt + 1) % len;

                    float r = Multiply(pts[ix1], pts[ix2], pts[ix0]);
                    if (r > 0) {
                        val++;
                        break;
                    } else if (r < 0) {
                        val--;
                        break;
                    }
                }
                if (val > 0)
                    ret = true;
                else
                    ret = false;
            }
            return ret;
        }
        /// <summary>
        /// Calculate polygon centroid and radius
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <param name="centroid"></param>
        /// <returns></returns>
        public static float CalcPolygonCentroidAndRadius(IList<Vector3> pts, int start, int len, out Vector3 centroid)
        {
            float ret = 0;
            if (len > 0) {
                float x = 0;
                float y = 0;
                for (int i = 1; i < len; ++i) {
                    int ix = start + i;
                    x += pts[ix].X;
                    y += pts[ix].Z;
                }
                x /= len;
                y /= len;
                centroid = new Vector3(x, 0, y);
                float distSqr = 0;
                for (int i = 1; i < len; ++i) {
                    int ix = start + i;
                    float tmp = Geometry.DistanceSquare(centroid, pts[ix]);
                    if (distSqr < tmp)
                        distSqr = tmp;
                }
                ret = (float)Math.Sqrt(distSqr);
            } else {
                centroid = new Vector3();
            }
            return ret;
        }
        /// <summary>
        /// Compute polygonal axis-aligned rectangular bounding box
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <param name="minXval"></param>
        /// <param name="maxXval"></param>
        /// <param name="minZval"></param>
        /// <param name="maxZval"></param>
        public static void CalcPolygonBound(IList<Vector3> pts, int start, int len, out float minXval, out float maxXval, out float minYval, out float maxYval)
        {
            maxXval = pts[start].X;
            minXval = pts[start].X;
            maxYval = pts[start].Z;
            minYval = pts[start].Z;
            for (int i = 1; i < len; ++i) {
                int ix = start + i;
                float xv = pts[ix].X;
                float yv = pts[ix].Z;
                if (maxXval < xv)
                    maxXval = xv;
                else if (minXval > xv)
                    minXval = xv;
                if (maxYval < yv)
                    maxYval = yv;
                else if (minYval > yv)
                    minYval = yv;
            }
        }
        /// <summary>
        /// Determine whether a point is within a polygon
        /// </summary>
        /// <typeparam name="PointT"></typeparam>
        /// <param name="pt"></param>
        /// <param name="pts"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns>
        /// 1  -- within polygon
        /// 0  -- on polygon edge
        /// -1 -- outside the polygon 
        /// </returns>
        public static int PointInPolygon(Vector3 pt, IList<Vector3> pts, int start, int len)
        {
            float maxXval;
            float minXval;
            float maxYval;
            float minYval;
            CalcPolygonBound(pts, start, len, out minXval, out maxXval, out minYval, out maxYval);
            return PointInPolygon(pt, pts, start, len, minXval, maxXval, minYval, maxYval);
        }

        public static int PointInPolygon(Vector3 pt, IList<Vector3> pts, int start, int len, float minXval, float maxXval, float minYval, float maxYval)
        {
            if ((pt.X > maxXval) | (pt.X < minXval) | pt.Z > maxYval | pt.Z < minYval) {
                return -1;//outside the polygon
            } else {
                int cnt = 0;
                int lastStatus;
                Vector3 pt0 = pts[start];
                if (pt0.Z - pt.Z > c_FloatPrecision)
                    lastStatus = 1;
                else if (pt0.Z - pt.Z < c_FloatPrecision)
                    lastStatus = -1;
                else
                    lastStatus = 0;

                for (int i = 0; i < len; ++i) {
                    int ix1 = start + i;
                    int ix2 = start + (i + 1) % len;
                    Vector3 pt1 = pts[ix1];
                    Vector3 pt2 = pts[ix2];

                    if (PointOnLine(pt, pt1, pt2))
                        return 0;

                    int status;
                    if (pt2.Z - pt.Z > c_FloatPrecision)
                        status = 1;
                    else if (pt2.Z - pt.Z < c_FloatPrecision)
                        status = -1;
                    else
                        status = 0;

                    int temp = status - lastStatus;
                    lastStatus = status;
                    if (temp > 0) {
                        if (!PointIsLeftOn(pt, pt1, pt2))
                            cnt += temp;
                    } else if (temp < 0) {
                        if (PointIsLeft(pt, pt1, pt2))
                            cnt += temp;
                    }
                }
                if (cnt == 0)
                    return -1;
                else
                    return 1;
            }
        }

        /// <summary>
        /// Determine the relationship between a point and the triangle formed by three other points
        /// Note: The three points A B C of the triangle must be arranged counterclockwise
        /// -1-outside the triangle 0-inside the triangle 1-on the triangle side ab 2-on the triangle side bc 3-on the triangle side ca 4-on the triangle vertex
        /// </summary>
        /// <param name="pn">point</param>
        /// <param name="pa">triangle vertex A</param>
        /// <param name="pb">triangle vertex B</param>
        /// <param name="pc">triangle vertex C</param>
        /// <returns>-1-Outside the triangle 0-Inside the triangle 1-On the triangle side ab 2-On the triangle side bc 3-On the triangle side ca 4-On the triangle vertex</returns>
        public static int PointInTriangle(Vector3 pn, Vector3 pa, Vector3 pb, Vector3 pc)
        {
            //First, determine whether it is on the edge
            //Note: The three points A B C of the triangle must be arranged counterclockwise
            int num = 0;
            int side = 0;
            if (PointOnLine(pn, pa, pb)) {
                side = 1;
                num++;
            }
            if (PointOnLine(pn, pb, pc)) {
                side = 2;
                num++;
            }
            if (PointOnLine(pn, pc, pa)) {
                side = 3;
                num++;
            }
            if (num > 1)
                return 4;
            else if (num == 1)
                return side;
            else {
                if (SignArea(pn, pa, pb) >= 0.0f && SignArea(pn, pb, pc) >= 0.0f && SignArea(pn, pc, pa) >= 0.0f)
                    return 0;
                else
                    return -1;
            }
        }
        /// <summary>
        /// To calculate the signed area of a triangle, enter the coordinates of the three vertices of the triangle.
        /// </summary>
        /// <param name="na"></param>
        /// <param name="nb"></param>
        /// <param name="nc"></param>
        /// <returns></returns>
        public static float SignArea(Vector3 na, Vector3 nb, Vector3 nc)
        {
            return ((nb.X - na.X) * (nc.Z - na.Z) - (nb.Z - na.Z) * (nc.X - na.X)) / 2;
        }
        /// <summary>
        /// Thin out a polygon or polyline.
        /// </summary>
        /// <typeparam name="PointT"></typeparam>
        /// <param name="pts"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <param name="delta">Distance to determine if three specified points are collinear</param>
        /// <returns></returns>
        public static IList<Vector3> Sparseness(IList<Vector3> pts, int start, int len, float delta)
        {
            Vector3 lastPt = pts[0];
            LinkedList<Vector3> links = new LinkedList<Vector3>();
            links.AddLast(lastPt);
            for (int i = 1; i < len; i++) {
                int ix = start + i;
                Vector3 pt = pts[ix];
                if (!PointOverlapPoint(pt, lastPt)) {
                    links.AddLast(pt);
                }
                lastPt = pt;
            }
            for (int count = 1; count > 0; ) {
                count = 0;
                LinkedListNode<Vector3> node = links.First;
                while (node != null) {
                    LinkedListNode<Vector3> node2 = node.Next;
                    LinkedListNode<Vector3> node3 = null;
                    if (node2 != null)
                        node3 = node2.Next;
                    if (node3 != null) {
                        double d = PointToLineDistance(node2.Value, node.Value, node3.Value);
                        if (d <= delta) {
                            links.Remove(node2);
                            count++;
                        }
                    }
                    //Regardless of whether to delete a node, always remove the next node,
                    //which can avoid some situations where arcs are drawn into straight lines.
                    node = node.Next;
                }
            }
            Vector3[] rpts = new Vector3[links.Count];
            links.CopyTo(rpts, 0);
            return rpts;
        }
    }
}

namespace ScriptRuntime
{
    public struct Vector2Int : IEquatable<Vector2Int>, IFormattable
    {
        private int m_X;

        private int m_Y;

        private static readonly Vector2Int s_Zero = new Vector2Int(0, 0);

        private static readonly Vector2Int s_One = new Vector2Int(1, 1);

        private static readonly Vector2Int s_Up = new Vector2Int(0, 1);

        private static readonly Vector2Int s_Down = new Vector2Int(0, -1);

        private static readonly Vector2Int s_Left = new Vector2Int(-1, 0);

        private static readonly Vector2Int s_Right = new Vector2Int(1, 0);

        public int x {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return m_X;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                m_X = value;
            }
        }

        public int y {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return m_Y;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                m_Y = value;
            }
        }

        public int this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return index switch {
                    0 => x,
                    1 => y,
                    _ => throw new IndexOutOfRangeException($"Invalid Vector2Int index addressed: {index}!"),
                };
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                switch (index) {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException($"Invalid Vector2Int index addressed: {index}!");
                }
            }
        }

        public float magnitude {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return MathF.Sqrt(x * x + y * y);
            }
        }

        public int sqrMagnitude {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return x * x + y * y;
            }
        }

        public static Vector2Int zero {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return s_Zero;
            }
        }

        public static Vector2Int one {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return s_One;
            }
        }

        public static Vector2Int up {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return s_Up;
            }
        }

        public static Vector2Int down {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return s_Down;
            }
        }

        public static Vector2Int left {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return s_Left;
            }
        }

        public static Vector2Int right {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return s_Right;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2Int(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector2Int a, Vector2Int b)
        {
            float diff_x = a.x - b.x;
            float diff_y = a.y - b.y;
            return (float)Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Min(Vector2Int lhs, Vector2Int rhs)
        {
            return new Vector2Int(Math.Min(lhs.x, rhs.y), Math.Min(lhs.y, rhs.y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Max(Vector2Int lhs, Vector2Int rhs)
        {
            return new Vector2Int(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Scale(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x * b.x, a.y * b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Scale(Vector2Int scale)
        {
            x *= scale.x;
            y *= scale.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(Vector2Int min, Vector2Int max)
        {
            x = Math.Max(min.x, x);
            x = Math.Min(max.x, x);
            y = Math.Max(min.y, y);
            y = Math.Min(max.y, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector2(Vector2Int v)
        {
            return new Vector2(v.x, v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector3Int(Vector2Int v)
        {
            return new Vector3Int(v.x, v.y, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int FloorToInt(Vector2 v)
        {
            return new Vector2Int((int)Math.Floor(v.X), (int)Math.Floor(v.Y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int CeilToInt(Vector2 v)
        {
            return new Vector2Int((int)Math.Ceiling(v.X), (int)Math.Ceiling(v.Y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int RoundToInt(Vector2 v)
        {
            return new Vector2Int((int)Math.Round(v.X), (int)Math.Round(v.Y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator -(Vector2Int v)
        {
            return new Vector2Int(-v.x, -v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x + b.x, a.y + b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x - b.x, a.y - b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator *(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x * b.x, a.y * b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator *(int a, Vector2Int b)
        {
            return new Vector2Int(a * b.x, a * b.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator *(Vector2Int a, int b)
        {
            return new Vector2Int(a.x * b, a.y * b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int operator /(Vector2Int a, int b)
        {
            return new Vector2Int(a.x / b, a.y / b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector2Int lhs, Vector2Int rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector2Int lhs, Vector2Int rhs)
        {
            return !(lhs == rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
        {
            if (!(other is Vector2Int)) {
                return false;
            }
            return Equals((Vector2Int)other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector2Int other)
        {
            return x == other.x && y == other.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return ToString(null, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider == null) {
                formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            }
            return string.Format("({0}, {1})", x.ToString(format, formatProvider), y.ToString(format, formatProvider));
        }
    }
    public struct Vector3Int : IEquatable<Vector3Int>, IFormattable
    {
        private int m_X;

        private int m_Y;

        private int m_Z;

        private static readonly Vector3Int s_Zero = new Vector3Int(0, 0, 0);

        private static readonly Vector3Int s_One = new Vector3Int(1, 1, 1);

        private static readonly Vector3Int s_Up = new Vector3Int(0, 1, 0);

        private static readonly Vector3Int s_Down = new Vector3Int(0, -1, 0);

        private static readonly Vector3Int s_Left = new Vector3Int(-1, 0, 0);

        private static readonly Vector3Int s_Right = new Vector3Int(1, 0, 0);

        private static readonly Vector3Int s_Forward = new Vector3Int(0, 0, 1);

        private static readonly Vector3Int s_Back = new Vector3Int(0, 0, -1);

        public int x {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return m_X;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                m_X = value;
            }
        }

        public int y {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return m_Y;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                m_Y = value;
            }
        }

        public int z {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return m_Z;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                m_Z = value;
            }
        }

        public int this[int index] {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return index switch {
                    0 => x,
                    1 => y,
                    2 => z,
                    _ => throw new IndexOutOfRangeException(string.Format("Invalid Vector3Int index addressed: {0}!", index)),
                };
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set {
                switch (index) {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException(string.Format("Invalid Vector3Int index addressed: {0}!", index));
                }
            }
        }

        public float magnitude {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return MathF.Sqrt(x * x + y * y + z * z);
            }
        }

        public int sqrMagnitude {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return x * x + y * y + z * z;
            }
        }

        public static Vector3Int zero {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return s_Zero;
            }
        }

        public static Vector3Int one {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return s_One;
            }
        }

        public static Vector3Int up {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return s_Up;
            }
        }

        public static Vector3Int down {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return s_Down;
            }
        }

        public static Vector3Int left {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return s_Left;
            }
        }

        public static Vector3Int right {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return s_Right;
            }
        }

        public static Vector3Int forward {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return s_Forward;
            }
        }

        public static Vector3Int back {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                return s_Back;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3Int(int x, int y)
        {
            m_X = x;
            m_Y = y;
            m_Z = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3Int(int x, int y, int z)
        {
            m_X = x;
            m_Y = y;
            m_Z = z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int x, int y, int z)
        {
            m_X = x;
            m_Y = y;
            m_Z = z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(Vector3Int a, Vector3Int b)
        {
            return (a - b).magnitude;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Min(Vector3Int lhs, Vector3Int rhs)
        {
            return new Vector3Int(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Max(Vector3Int lhs, Vector3Int rhs)
        {
            return new Vector3Int(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Scale(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Scale(Vector3Int scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clamp(Vector3Int min, Vector3Int max)
        {
            x = Math.Max(min.x, x);
            x = Math.Min(max.x, x);
            y = Math.Max(min.y, y);
            y = Math.Min(max.y, y);
            z = Math.Max(min.z, z);
            z = Math.Min(max.z, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Vector3(Vector3Int v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Vector2Int(Vector3Int v)
        {
            return new Vector2Int(v.x, v.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int FloorToInt(Vector3 v)
        {
            return new Vector3Int((int)Math.Floor(v.X), (int)Math.Floor(v.Y), (int)Math.Floor(v.Z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int CeilToInt(Vector3 v)
        {
            return new Vector3Int((int)Math.Ceiling(v.X), (int)Math.Ceiling(v.Y), (int)Math.Ceiling(v.Z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int RoundToInt(Vector3 v)
        {
            return new Vector3Int((int)Math.Round(v.X), (int)Math.Round(v.Y), (int)Math.Round(v.Z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int operator +(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int operator -(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int operator *(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int operator -(Vector3Int a)
        {
            return new Vector3Int(-a.x, -a.y, -a.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int operator *(Vector3Int a, int b)
        {
            return new Vector3Int(a.x * b, a.y * b, a.z * b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int operator *(int a, Vector3Int b)
        {
            return new Vector3Int(a * b.x, a * b.y, a * b.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int operator /(Vector3Int a, int b)
        {
            return new Vector3Int(a.x / b, a.y / b, a.z / b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector3Int lhs, Vector3Int rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Vector3Int lhs, Vector3Int rhs)
        {
            return !(lhs == rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
        {
            if (!(other is Vector3Int)) {
                return false;
            }
            return Equals((Vector3Int)other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Vector3Int other)
        {
            return this == other;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            int yHash = y.GetHashCode();
            int zHash = z.GetHashCode();
            return x.GetHashCode() ^ (yHash << 4) ^ (yHash >> 28) ^ (zHash >> 4) ^ (zHash << 28);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return ToString(null, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider == null) {
                formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            }
            return string.Format("({0}, {1}, {2})", x.ToString(format, formatProvider), y.ToString(format, formatProvider), z.ToString(format, formatProvider));
        }
    }
}
