using System.Collections.Generic;
using System;
using ScriptRuntime;

namespace GameFramework
{
    public class KdTreeObject
    {
        public EntityInfo Object;
        public Vector3 Position;
        public float Radius;

        internal float MaxX;
        internal float MinX;
        internal float MaxZ;
        internal float MinZ;
        internal bool Indexed;

        public KdTreeObject(EntityInfo obj)
        {
            CopyFrom(obj);
        }
        public void CopyFrom(EntityInfo obj)
        {
            if (null != obj) {
                Object = obj;
                Position = obj.GetMovementStateInfo().GetPosition3D();
                Radius = obj.GetRadius();
                MaxX = Position.X + Radius;
                MinX = Position.X - Radius;
                MaxZ = Position.Z + Radius;
                MinZ = Position.Z - Radius;
                Indexed = false;
            } else {
                Object = null;
                Position = new Vector3();
                Radius = 0;
                MaxX = MinX = 0;
                MaxZ = MinZ = 0;
                Indexed = false;
            }
        }
    }
    public sealed class KdObjectTree
    {
        public const int c_MaxLeafSize = 4;

        private struct KdTreeNode
        {
            internal int m_Begin;
            internal int m_End;
            internal int m_Left;
            internal int m_Right;
            internal float m_MaxX;
            internal float m_MaxZ;
            internal float m_MinX;
            internal float m_MinZ;
        }

        public void FullBuild(IList<EntityInfo> objs)
        {
            BeginBuild(objs.Count);
            for (int ix = 0; ix < objs.Count; ++ix) {
                AddObjForBuild(objs[ix]);
            }
            EndBuild();
        }
        public void Clear()
        {
            if (null != m_Objects) {
                m_ObjectNum = 0;
                for (int i = 0; i < m_Objects.Length; ++i) {
                    var obj = m_Objects[i];
                    if (null != obj) {
                        obj.Object = null;
                    }
                }
            }
        }
        public void BeginBuild(int count)
        {
            if (null == m_Objects || m_Objects.Length < count) {
                m_Objects = new KdTreeObject[count * 2];
            }
            m_ObjectNum = 0;
            for (int i = 0; i < m_Objects.Length; ++i) {
                var obj = m_Objects[i];
                if (null != obj) {
                    obj.Object = null;
                }
            }
        }
        public void AddObjForBuild(EntityInfo obj)
        {
            if (m_ObjectNum >= m_Objects.Length)
                return;
            if (null == m_Objects[m_ObjectNum])
                m_Objects[m_ObjectNum] = new KdTreeObject(obj);
            else
                m_Objects[m_ObjectNum].CopyFrom(obj);
            ++m_ObjectNum;
        }
        public void EndBuild()
        {
            if (m_ObjectNum > 0) {
                if (null == m_KdTree || m_KdTree.Length < 3 * m_ObjectNum) {
                    m_KdTree = new KdTreeNode[3 * m_ObjectNum];
                    for (int i = 0; i < m_KdTree.Length; ++i) {
                        m_KdTree[i] = new KdTreeNode();
                    }
                }
                m_MaxNodeNum = 2 * m_ObjectNum;
                BuildImpl();
            }
        }

        public void QueryWithAction(float x, float y, float z, float range, MyAction<float, KdTreeObject> visitor)
        {
            QueryWithAction(new Vector3(x, y, z), range, visitor);
        }

        public void QueryWithAction(EntityInfo obj, float range, MyAction<float, KdTreeObject> visitor)
        {
            QueryWithAction(obj.GetMovementStateInfo().GetPosition3D(), range, visitor);
        }

        public void QueryWithAction(Vector3 pos, float range, MyAction<float, KdTreeObject> visitor)
        {
            if (null != m_KdTree && m_ObjectNum > 0 && m_KdTree.Length > 0) {
                float rangeSq = Sqr(range);
                QueryImpl(pos, range, rangeSq, (float distSqr, KdTreeObject obj) => { visitor(distSqr, obj); return true; });
            }
        }

        public void QueryWithFunc(float x, float y, float z, float range, MyFunc<float, KdTreeObject, bool> visitor)
        {
            QueryWithFunc(new Vector3(x, y, z), range, visitor);
        }

        public void QueryWithFunc(EntityInfo obj, float range, MyFunc<float, KdTreeObject, bool> visitor)
        {
            QueryWithFunc(obj.GetMovementStateInfo().GetPosition3D(), range, visitor);
        }

        public void QueryWithFunc(Vector3 pos, float range, MyFunc<float, KdTreeObject, bool> visitor)
        {
            if (null != m_KdTree && m_ObjectNum > 0 && m_KdTree.Length > 0) {
                float rangeSq = Sqr(range);
                QueryImpl(pos, range, rangeSq, visitor);
            }
        }

        public void VisitTreeWithAction(MyAction<float, float, float, float, int, int, KdTreeObject[]> visitor)
        {
            VisitTreeWithFunc((float minx, float minz, float maxx, float maxz, int begin, int end, KdTreeObject[] objs) => { visitor(minx, minz, maxx, maxz, begin, end, objs); return true; });
        }

        public void VisitTreeWithFunc(MyFunc<float, float, float, float, int, int, KdTreeObject[], bool> visitor)
        {
            if (null != m_KdTree && m_ObjectNum > 0 && m_KdTree.Length > 0) {
                VisitTreeImpl(visitor);
            }
        }

        private void BuildImpl()
        {
            int nextUnusedNode = 1;
            m_BuildStack.Push(0);
            m_BuildStack.Push(m_ObjectNum);
            m_BuildStack.Push(0);
            while (m_BuildStack.Count >= 3) {
                int begin = m_BuildStack.Pop(); //The starting position of the data object to be classified
                int end = m_BuildStack.Pop();   //The position after the end position of the data object to be classified
                int node = m_BuildStack.Pop();  //The location on the kdtree used to construct the new node

                KdTreeObject obj0 = m_Objects[begin];
                float minX = obj0.MinX;
                float maxX = obj0.MaxX;
                float minZ = obj0.MinZ;
                float maxZ = obj0.MaxZ;
                for (int i = begin + 1; i < end; ++i) {
                    KdTreeObject obj = m_Objects[i];
                    float newMaxX = obj.MaxX;
                    float newMinX = obj.MinX;
                    float newMaxZ = obj.MaxZ;
                    float newMinZ = obj.MinZ;
                    if (minX > newMinX) minX = newMinX;
                    if (maxX < newMaxX) maxX = newMaxX;
                    if (minZ > newMinZ) minZ = newMinZ;
                    if (maxZ < newMaxZ) maxZ = newMaxZ;
                }
                m_KdTree[node].m_MinX = minX;
                m_KdTree[node].m_MaxX = maxX;
                m_KdTree[node].m_MinZ = minZ;
                m_KdTree[node].m_MaxZ = maxZ;

                if (end - begin > c_MaxLeafSize) {
                    //Position reservation of 2 child nodes on kdtree
                    m_KdTree[node].m_Left = nextUnusedNode;
                    ++nextUnusedNode;
                    m_KdTree[node].m_Right = nextUnusedNode;
                    ++nextUnusedNode;

                    bool isVertical = (maxX - minX > maxZ - minZ);
                    float splitValue = (isVertical ? 0.5f * (maxX + minX) : 0.5f * (maxZ + minZ));

                    int begin0 = begin;
                    int left = begin;
                    int right = end;

                    //Next, the meaning of the variables is as follows:
                    //begin0 is the starting position of the data object hanging on the current node
                    //begin is the position after the end position of the data object hanging on the current node,
                    // and is also the starting position of the data object of the left subtree.
                    //left is the position after the end position of the data object of the left subtree,
                    // and is also the starting position of the data object to be classified
                    //right is the starting position of the currently determined right subtree data object
                    //end is the position after the end position of the right subtree data object

                    bool canSplit = false;
                    while (left < right) {
                        while (left < right) {
                            KdTreeObject obj = m_Objects[left];
                            if ((isVertical ? obj.MaxX : obj.MaxZ) < splitValue) {
                                //obj is the data object on the left subtree, marking the subtree to be split
                                ++left;
                                canSplit = true;
                            } else if ((isVertical ? obj.MinX : obj.MinZ) <= splitValue) {
                                //obj is the data object on the current node. The data of begin and
                                //the position of begin need to be adjusted later.
                                obj.Indexed = true;
                                break;
                            } else {
                                break;
                            }
                        }
                        while (left < right) {
                            KdTreeObject obj = m_Objects[right - 1];
                            if ((isVertical ? obj.MinX : obj.MinZ) > splitValue) {
                                //obj is the data object on the right subtree. There is no need
                                //to mark the split here.
                                --right;
                            } else if ((isVertical ? obj.MaxX : obj.MaxZ) >= splitValue) {
                                //obj is the data object on the current node. The data of begin
                                //and the position of begin need to be adjusted later.
                                obj.Indexed = true;
                                break;
                            } else {
                                break;
                            }
                        }

                        if (left < right) {
                            if (m_Objects[left].Indexed || m_Objects[right - 1].Indexed) {
                                if (m_Objects[left].Indexed) {
                                    KdTreeObject tmp = m_Objects[begin];
                                    m_Objects[begin] = m_Objects[left];
                                    m_Objects[left] = tmp;
                                    ++begin;
                                    ++left;
                                    canSplit = true;
                                    //Hang the data object on the current node (data is exchanged to the begin position),
                                    //begin is moved back one position, and left is also moved back one position.
                                }
                                if (left < right && m_Objects[right - 1].Indexed) {
                                    KdTreeObject tmp = m_Objects[begin];
                                    m_Objects[begin] = m_Objects[right - 1];
                                    m_Objects[right - 1] = m_Objects[left];
                                    m_Objects[left] = tmp;
                                    ++begin;
                                    ++left;
                                    canSplit = true;
                                    //Hang the data object on the current node (data is exchanged to the begin position),
                                    //the data object at the left position is placed on right-1 (continue processing),
                                    //begin is moved one position back, and left is also moved one position back.
                                }
                                //After processing the data to be linked, continue processing (there may be data at the
                                //left or right-1 position that does not meet the classification standards)
                            } else {
                                KdTreeObject tmp = m_Objects[left];
                                m_Objects[left] = m_Objects[right - 1];
                                m_Objects[right - 1] = tmp;
                                ++left;
                                --right;
                                canSplit = true;
                                //The left and right-1 positions are data that do not meet the classification standards.
                                //Continue processing after exchanging data.
                            }
                        }
                    }

                    if (canSplit) {
                        m_KdTree[node].m_Begin = begin0;
                        m_KdTree[node].m_End = begin;

                        if (left > begin) {
                            m_BuildStack.Push(m_KdTree[node].m_Left);
                            m_BuildStack.Push(left);
                            m_BuildStack.Push(begin);
                        } else {
                            m_KdTree[node].m_Left = 0;
                        }

                        if (end > left) {
                            m_BuildStack.Push(m_KdTree[node].m_Right);
                            m_BuildStack.Push(end);
                            m_BuildStack.Push(left);
                        } else {
                            m_KdTree[node].m_Right = 0;
                        }
                    } else {
                        m_KdTree[node].m_Begin = begin0;
                        m_KdTree[node].m_End = begin0;
                        m_KdTree[node].m_Left = 0;
                        m_KdTree[node].m_Right = 0;
                        nextUnusedNode -= 2;

                        LogSystem.Error("KdTree build error, node {0} has no objs [{1}-{1}) and no sub tree.", node, begin0);
                    }
                } else {
                    m_KdTree[node].m_Begin = begin;
                    m_KdTree[node].m_End = end;
                    m_KdTree[node].m_Left = 0;
                    m_KdTree[node].m_Right = 0;
                }
            }
        }

        private void QueryImpl(Vector3 pos, float range, float rangeSq, MyFunc<float, KdTreeObject, bool> visitor)
        {
            m_QueryStack.Push(0);
            while (m_QueryStack.Count > 0) {
                int node = m_QueryStack.Pop();
                int begin = m_KdTree[node].m_Begin;
                int end = m_KdTree[node].m_End;
                int left = m_KdTree[node].m_Left;
                int right = m_KdTree[node].m_Right;

                if (end > begin) {
                    for (int i = begin; i < end; ++i) {
                        KdTreeObject obj = m_Objects[i];
                        if (Geometry.RectangleOverlapRectangle(pos.X - range, pos.Z - range, pos.X + range, pos.Z + range, obj.MinX, obj.MinZ, obj.MaxX, obj.MaxZ)) {
                            float distSq = Geometry.DistanceSquare(pos, obj.Position);
                            if (!Visit(visitor, distSq, obj)) {
                                m_QueryStack.Clear();
                                return;
                            }
                        }
                    }
                }

                float minX = m_KdTree[node].m_MinX;
                float minZ = m_KdTree[node].m_MinZ;
                float maxX = m_KdTree[node].m_MaxX;
                float maxZ = m_KdTree[node].m_MaxZ;

                bool isVertical = (maxX - minX > maxZ - minZ);
                float splitValue = (isVertical ? 0.5f * (maxX + minX) : 0.5f * (maxZ + minZ));

                if ((isVertical ? pos.X + range : pos.Z + range) < splitValue) {
                    if (left > 0)
                        m_QueryStack.Push(left);
                } else if ((isVertical ? pos.X - range : pos.Z - range) <= splitValue) {
                    if (left > 0)
                        m_QueryStack.Push(left);
                    if (right > 0)
                        m_QueryStack.Push(right);
                } else {
                    if (right > 0)
                        m_QueryStack.Push(right);
                }
            }
        }
        private bool Visit(MyFunc<float, KdTreeObject, bool> visitor, float distSqr, KdTreeObject obj)
        {
            try {
                return visitor(distSqr, obj);
            }catch(Exception ex) {
                LogSystem.Error("exception:{0}\n{1}", ex.Message, ex.StackTrace);
                return false;
            }
        }

        private void VisitTreeImpl(MyFunc<float, float, float, float, int, int, KdTreeObject[], bool> visitor)
        {
            m_QueryStack.Push(0);
            while (m_QueryStack.Count > 0) {
                int node = m_QueryStack.Pop();

                int begin = m_KdTree[node].m_Begin;
                int end = m_KdTree[node].m_End;
                int left = m_KdTree[node].m_Left;
                int right = m_KdTree[node].m_Right;

                float minX = m_KdTree[node].m_MinX;
                float minZ = m_KdTree[node].m_MinZ;
                float maxX = m_KdTree[node].m_MaxX;
                float maxZ = m_KdTree[node].m_MaxZ;

                bool isVertical = (maxX - minX > maxZ - minZ);
                if (isVertical) {
                    float splitValue = 0.5f * (maxX + minX);
                    if (!Visit(visitor, splitValue, minZ, splitValue, maxZ, begin, end, m_Objects)) {
                        m_QueryStack.Clear();
                        return;
                    }
                } else {
                    float splitValue = 0.5f * (maxZ + minZ);
                    if (!Visit(visitor, minX, splitValue, maxX, splitValue, begin, end, m_Objects)) {
                        m_QueryStack.Clear();
                        return;
                    }
                }

                if (left > 0)
                    m_QueryStack.Push(left);
                if (right > 0)
                    m_QueryStack.Push(right);
            }
        }
        private bool Visit(MyFunc<float, float, float, float, int, int, KdTreeObject[], bool> visitor, float minx, float minz, float maxx, float maxz, int begin, int end, KdTreeObject[] objs)
        {
            try {
                return visitor(minx, minz, maxx, maxz, begin, end, objs);
            }
            catch (Exception ex) {
                LogSystem.Error("exception:{0}\n{1}", ex.Message, ex.StackTrace);
                return false;
            }
        }

        private static float Sqr(float v)
        {
            return v * v;
        }

        private static float CalcSquareDistToRectangle(float distMinX, float distMaxX, float distMinZ, float distMaxZ)
        {
            float ret = 0;
            if (distMinX > 0) ret += distMinX * distMinX;
            if (distMaxX > 0) ret += distMaxX * distMaxX;
            if (distMinZ > 0) ret += distMinZ * distMinZ;
            if (distMaxZ > 0) ret += distMaxZ * distMaxZ;
            return ret;
        }

        private KdTreeObject[] m_Objects = null;
        private int m_ObjectNum = 0;
        private KdTreeNode[] m_KdTree = null;
        private int m_MaxNodeNum = 0;
        private Stack<int> m_BuildStack = new Stack<int>(4096);
        private Stack<int> m_QueryStack = new Stack<int>(4096);
    }
}
