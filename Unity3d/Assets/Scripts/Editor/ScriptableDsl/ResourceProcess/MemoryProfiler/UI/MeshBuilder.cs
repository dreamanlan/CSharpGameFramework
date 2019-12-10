using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Unity.MemoryProfilerForExtension.Editor.UI
{
    internal class Mesh2D
    {
        public List<Mesh> meshes = new List<Mesh>();

        public void Render()
        {
            Material mat = (Material)EditorGUIUtility.LoadRequired("SceneView/2DHandleLines.mat");
            mat.SetPass(0);
            for (int i = 0; i < meshes.Count; i++)
            {
                Graphics.DrawMeshNow(meshes[i], Handles.matrix);
            }
        }

        public void CleanupMeshes()
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                UnityEngine.Object.DestroyImmediate(meshes[i]);
            }
            meshes.Clear();
        }
    }

    /// <summary>
    /// Helper class that convert lists of triangle/rectangle/lines into a renderable mesh
    /// </summary>
    internal class MeshBuilder
    {
        public class Layer
        {
            public List<Mesh> meshes = new List<Mesh>();
            public List<Triangle> triangle = new List<Triangle>();
            public List<Rectangle> rectangle_filled = new List<Rectangle>();
            public List<Rectangle> rectangle_outline = new List<Rectangle>();
            public List<Line> line = new List<Line>();
            public class MeshDataTriangle
            {
                public const int kVertexPerTriangle = 3;
                public const int kIndexPerTriangle = 3;
                public const int kVertexPerRectangle = 4;
                public const int kIndexPerRectangle = 6;
                public Vector3[] vertex;
                public Color[] color;
                public int[] index;
                public int cur_Vertex;
                public int cur_Index;
                public MeshDataTriangle(int triangleCount, int RectangleCount)
                {
                    int vertexCount = triangleCount * 3 + RectangleCount * 4;
                    int indexCount = triangleCount * 3 + RectangleCount * 6;
                    vertex = new Vector3[vertexCount];
                    color = new Color[vertexCount];
                    index = new int[indexCount];
                    cur_Vertex = 0;
                    cur_Index = 0;
                }

                public void Add(List<Triangle> elems, int iFirst, int iLast)
                {
                    int count = iLast - iFirst;
                    for (int i = 0; i != count; ++i)
                    {
                        var e = elems[iFirst + i];
                        int i3 = i * 3;
                        vertex[cur_Vertex + i3 + 0] = new Vector3(e.pos0.x, e.pos0.y, 0f);
                        vertex[cur_Vertex + i3 + 1] = new Vector3(e.pos1.x, e.pos1.y, 0f);
                        vertex[cur_Vertex + i3 + 2] = new Vector3(e.pos2.x, e.pos2.y, 0f);

                        color[cur_Vertex + i3 + 0] = e.color0;
                        color[cur_Vertex + i3 + 1] = e.color1;
                        color[cur_Vertex + i3 + 2] = e.color2;

                        index[cur_Index + i3 + 0] = cur_Vertex + i3 + 0;
                        index[cur_Index + i3 + 1] = cur_Vertex + i3 + 1;
                        index[cur_Index + i3 + 2] = cur_Vertex + i3 + 2;
                    }
                    cur_Vertex += count * 3;
                    cur_Index += count * 3;
                }

                public void Add(List<Rectangle> elems, int iFirst, int iLast)
                {
                    int count = iLast - iFirst;
                    for (int i = 0; i != count; ++i)
                    {
                        var e = elems[iFirst + i];
                        int i4 = i * 4;
                        int i6 = i * 6;
                        vertex[cur_Vertex + i4 + 0] = new Vector3(e.pos0.x, e.pos0.y, 0f);
                        vertex[cur_Vertex + i4 + 1] = new Vector3(e.pos1.x, e.pos0.y, 0f);
                        vertex[cur_Vertex + i4 + 2] = new Vector3(e.pos1.x, e.pos1.y, 0f);
                        vertex[cur_Vertex + i4 + 3] = new Vector3(e.pos0.x, e.pos1.y, 0f);

                        color[cur_Vertex + i4 + 0] = e.color00;
                        color[cur_Vertex + i4 + 1] = e.color10;
                        color[cur_Vertex + i4 + 2] = e.color11;
                        color[cur_Vertex + i4 + 3] = e.color01;

                        index[cur_Index + i6 + 0] = cur_Vertex + i4 + 0;
                        index[cur_Index + i6 + 1] = cur_Vertex + i4 + 1;
                        index[cur_Index + i6 + 2] = cur_Vertex + i4 + 3;
                        index[cur_Index + i6 + 3] = cur_Vertex + i4 + 1;
                        index[cur_Index + i6 + 4] = cur_Vertex + i4 + 2;
                        index[cur_Index + i6 + 5] = cur_Vertex + i4 + 3;
                    }
                    cur_Vertex += count * 4;
                    cur_Index += count * 6;
                }

                public Mesh CreateMesh()
                {
                    Mesh mesh = new Mesh();
                    mesh.hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy | HideFlags.NotEditable;
                    mesh.vertices = vertex;
                    mesh.colors = color;
                    mesh.SetIndices(index, MeshTopology.Triangles, 0);
                    return mesh;
                }
            }
            public class MeshDataLine
            {
                public const int kVertexPerLine = 2;
                public const int kIndexPerLine = 2;
                public const int kVertexPerRectangle = 4;
                public const int kIndexPerRectangle = 8;
                public Vector3[] vertex;
                public Color[] color;
                public int[] index;
                public int cur_Vertex;
                public int cur_Index;

                public MeshDataLine(int lineCount, int RectangleCount)
                {
                    int vertexCount = lineCount * 2 + RectangleCount * 4;
                    int indexCount = lineCount * 2 + RectangleCount * 8;
                    vertex = new Vector3[vertexCount];
                    color = new Color[vertexCount];
                    index = new int[indexCount];
                    cur_Vertex = 0;
                    cur_Index = 0;
                }

                public void Add(List<Line> elems, int iFirst, int iLast)
                {
                    int count = iLast - iFirst;
                    for (int i = 0; i != count; ++i)
                    {
                        var e = elems[iFirst + i];
                        int i2 = i * 2;
                        vertex[cur_Vertex + i2 + 0] = new Vector3(e.pos0.x, e.pos0.y, 0f);
                        vertex[cur_Vertex + i2 + 1] = new Vector3(e.pos1.x, e.pos1.y, 0f);

                        color[cur_Vertex + i2 + 0] = e.color0;
                        color[cur_Vertex + i2 + 1] = e.color1;

                        index[cur_Index + i2 + 0] = cur_Vertex + i2 + 0;
                        index[cur_Index + i2 + 1] = cur_Vertex + i2 + 1;
                    }
                    cur_Vertex += count * 2;
                    cur_Index += count * 2;
                }

                public void Add(List<Rectangle> elems, int iFirst, int iLast)
                {
                    int count = iLast - iFirst;
                    for (int i = 0; i != count; ++i)
                    {
                        var e = elems[iFirst + i];
                        int i4 = i * 4;
                        int i8 = i * 8;
                        vertex[cur_Vertex + i4 + 0] = new Vector3(e.pos0.x, e.pos0.y, 0f);
                        vertex[cur_Vertex + i4 + 1] = new Vector3(e.pos1.x, e.pos0.y, 0f);
                        vertex[cur_Vertex + i4 + 2] = new Vector3(e.pos1.x, e.pos1.y, 0f);
                        vertex[cur_Vertex + i4 + 3] = new Vector3(e.pos0.x, e.pos1.y, 0f);

                        color[cur_Vertex + i4 + 0] = e.color00;
                        color[cur_Vertex + i4 + 1] = e.color10;
                        color[cur_Vertex + i4 + 2] = e.color11;
                        color[cur_Vertex + i4 + 3] = e.color01;

                        index[cur_Index + i8 + 0] = cur_Vertex + i4 + 0;
                        index[cur_Index + i8 + 1] = cur_Vertex + i4 + 1;
                        index[cur_Index + i8 + 2] = cur_Vertex + i4 + 1;
                        index[cur_Index + i8 + 3] = cur_Vertex + i4 + 2;
                        index[cur_Index + i8 + 4] = cur_Vertex + i4 + 2;
                        index[cur_Index + i8 + 5] = cur_Vertex + i4 + 3;
                        index[cur_Index + i8 + 6] = cur_Vertex + i4 + 3;
                        index[cur_Index + i8 + 7] = cur_Vertex + i4 + 0;
                    }
                    cur_Vertex += count * 4;
                    cur_Index += count * 8;
                }

                public Mesh CreateMesh()
                {
                    Mesh mesh = new Mesh();
                    mesh.hideFlags = HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy | HideFlags.NotEditable;
                    mesh.vertices = vertex;
                    mesh.colors = color;
                    mesh.SetIndices(index, MeshTopology.Lines, 0);
                    return mesh;
                }
            }

            const int kMaxVertex = 30000;
            const int kMaxIndex = 30000;
            private int ProcessShapes(ref int curAvailableShape, ref int curAvailableVertex, ref int curAvailableIndex, int VertexPerShape, int IndexPerShape)
            {
                var maxForVertex = curAvailableVertex / VertexPerShape;
                var maxForIndex = curAvailableIndex / IndexPerShape;
                var numToProcess = Math.Min(Math.Min(maxForIndex, maxForVertex), curAvailableShape);
                if (numToProcess > 0)
                {
                    curAvailableVertex -= numToProcess * VertexPerShape;
                    curAvailableIndex -= numToProcess * IndexPerShape;
                    curAvailableShape -= numToProcess;
                }
                return numToProcess;
            }

            public void Flush()
            {
                int curTriangle = 0;
                int curRectFilled = 0;


                int curAvailableTriangle = triangle.Count;
                int curAvailableRectFilled = rectangle_filled.Count;

                int curAvailableVertex = kMaxVertex;
                int curAvailableIndex = kMaxIndex;

                while (true)
                {
                    int numTriangle = ProcessShapes(ref curAvailableTriangle, ref curAvailableVertex, ref curAvailableIndex, MeshDataTriangle.kVertexPerTriangle, MeshDataTriangle.kIndexPerTriangle);
                    int numRectFilled = ProcessShapes(ref curAvailableRectFilled, ref curAvailableVertex, ref curAvailableIndex, MeshDataTriangle.kVertexPerRectangle, MeshDataTriangle.kIndexPerRectangle);

                    if (numTriangle > 0 || numRectFilled > 0)
                    {
                        MeshDataTriangle mdt = new MeshDataTriangle(numTriangle, numRectFilled);
                        if (numTriangle > 0)
                        {
                            mdt.Add(triangle, curTriangle, curTriangle + numTriangle);
                            curTriangle += numTriangle;
                        }
                        if (numRectFilled > 0)
                        {
                            mdt.Add(rectangle_filled, curRectFilled, curRectFilled + numRectFilled);
                            curRectFilled += numRectFilled;
                        }
                        meshes.Add(mdt.CreateMesh());
                        curAvailableVertex = kMaxVertex;
                        curAvailableIndex = kMaxIndex;
                    }
                    else
                    {
                        break;
                    }
                }

                int curLine = 0;
                int curRectOutline = 0;
                int curAvailableLine = line.Count;
                int curAvailableRectOutline = rectangle_outline.Count;
                while (true)
                {
                    int numLine = ProcessShapes(ref curAvailableLine, ref curAvailableVertex, ref curAvailableIndex, MeshDataLine.kVertexPerLine, MeshDataLine.kIndexPerLine);
                    int numRectOutline = ProcessShapes(ref curAvailableRectOutline, ref curAvailableVertex, ref curAvailableIndex, MeshDataLine.kVertexPerRectangle, MeshDataLine.kIndexPerRectangle);

                    if (numLine > 0 || numRectOutline > 0)
                    {
                        MeshDataLine mdl = new MeshDataLine(numLine, numRectOutline);
                        if (numLine > 0)
                        {
                            mdl.Add(line, curLine, curLine + numLine);
                            curLine += numLine;
                        }
                        if (numRectOutline > 0)
                        {
                            mdl.Add(rectangle_outline, curRectOutline, curRectOutline + numRectOutline);
                            curRectOutline += numRectOutline;
                        }
                        meshes.Add(mdl.CreateMesh());
                        curAvailableVertex = kMaxVertex;
                        curAvailableIndex = kMaxIndex;
                    }
                    else
                    {
                        break;
                    }
                }


                triangle.Clear();
                rectangle_filled.Clear();
                rectangle_outline.Clear();
                line.Clear();
            }
        }
        public class Triangle
        {
            public Triangle(Vector2 a, Vector2 b, Vector2 c, Color color)
            {
                pos0 = a;
                pos1 = b;
                pos2 = c;
                color0 = color;
                color1 = color;
                color2 = color;
            }

            public Triangle(Vector2 a, Vector2 b, Vector2 c, Color colorA, Color colorB, Color colorC)
            {
                pos0 = a;
                pos1 = b;
                pos2 = c;
                color0 = colorA;
                color1 = colorB;
                color2 = colorC;
            }

            public Vector2 pos0;
            public Vector2 pos1;
            public Vector2 pos2;
            public Color color0;
            public Color color1;
            public Color color2;
        }
        public class Rectangle
        {
            const float fEndPixelOffset = float.Epsilon;

            public Rectangle(Rect r /*, Vector2 pixelSize*/, Color c)
            {
                var r2 = r;// FixRect(r, pixelSize);
                pos0 = new Vector2(r2.xMin, r2.yMin);
                pos1 = new Vector2(r2.xMax, r2.yMax);
                color00 = c;
                color10 = c;
                color01 = c;
                color11 = c;
            }

            public Rectangle(Rect r /*, Vector2 pixelSize*/, Color c00, Color c10, Color c11, Color c01)
            {
                var r2 = r;// FixRect(r, pixelSize);
                pos0 = new Vector2(r2.xMin, r2.yMin);
                pos1 = new Vector2(r2.xMax, r2.yMax);
                color00 = c00;
                color10 = c10;
                color01 = c01;
                color11 = c11;
            }

            public Vector2 pos0;
            public Vector2 pos1;
            public Color color00;
            public Color color10;
            public Color color01;
            public Color color11;
        }
        public class Line
        {
            public Line(Vector2 p0, Vector2 p1, Color c)
            {
                pos0 = p0;
                pos1 = p1;
                color0 = c;
                color1 = c;
            }

            public Line(Vector2 p0, Vector2 p1, Color c0, Color c1)
            {
                pos0 = p0;
                pos1 = p1;
                color0 = c0;
                color1 = c1;
            }

            public Vector2 pos0;
            public Vector2 pos1;
            public Color color0;
            public Color color1;
        }
        public SortedDictionary<int, Layer> layer = new SortedDictionary<int, Layer>();
        public Layer GetLayer(int layerIndex)
        {
            Layer l;
            if (!layer.TryGetValue(layerIndex, out l))
            {
                l = new Layer();
                layer[layerIndex] = l;
            }
            return l;
        }

        public void Add(int layer, Triangle a)
        {
            GetLayer(layer).triangle.Add(a);
        }

        public void Add(int layer, Line a)
        {
            GetLayer(layer).line.Add(a);
        }

        public void Add(int layer, Rectangle a, bool filled)
        {
            if (filled)
            {
                GetLayer(layer).rectangle_filled.Add(a);
            }
            else
            {
                GetLayer(layer).rectangle_outline.Add(a);
            }
        }

        public void Flush()
        {
            foreach (var l in layer.Values)
            {
                l.Flush();
            }
        }

        public Mesh2D CreateMesh()
        {
            Mesh2D o = new Mesh2D();
            foreach (var l in layer.Values)
            {
                l.Flush();
                o.meshes.AddRange(l.meshes);
            }
            return o;
        }
    }
}
