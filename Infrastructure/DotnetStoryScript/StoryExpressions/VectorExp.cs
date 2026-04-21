using System;
using System.Collections.Generic;
using DotnetStoryScript.DslExpression;
using ScriptRuntime;
using ScriptableFramework;

namespace DotnetStoryScript
{
    // ========== Vector Constructors ==========

    /// <summary>
    /// vector2(x, y) - create Vector2
    /// </summary>
    internal sealed class Vector2Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: vector2(x, y)");

            float x = operands[0].GetFloat();
            float y = operands[1].GetFloat();
            Vector2Obj vecObj = new Vector2Obj { Value = new Vector2(x, y) };
            return BoxedValue.FromObject(vecObj);
        }
    }

    /// <summary>
    /// vector3(x, y, z) - create Vector3
    /// </summary>
    internal sealed class Vector3Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 3)
                throw new Exception("Expected: vector3(x, y, z)");

            float x = operands[0].GetFloat();
            float y = operands[1].GetFloat();
            float z = operands[2].GetFloat();
            Vector3Obj vecObj = new Vector3Obj { Value = new Vector3(x, y, z) };
            return BoxedValue.FromObject(vecObj);
        }
    }

    /// <summary>
    /// vector4(x, y, z, w) - create Vector4
    /// </summary>
    internal sealed class Vector4Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 4)
                throw new Exception("Expected: vector4(x, y, z, w)");

            float x = operands[0].GetFloat();
            float y = operands[1].GetFloat();
            float z = operands[2].GetFloat();
            float w = operands[3].GetFloat();
            Vector4Obj vecObj = new Vector4Obj { Value = new Vector4(x, y, z, w) };
            return BoxedValue.FromObject(vecObj);
        }
    }

    /// <summary>
    /// quaternion(x, y, z, w) - create Quaternion
    /// </summary>
    internal sealed class QuaternionExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 4)
                throw new Exception("Expected: quaternion(x, y, z, w)");

            float x = operands[0].GetFloat();
            float y = operands[1].GetFloat();
            float z = operands[2].GetFloat();
            float w = operands[3].GetFloat();
            QuaternionObj qObj = new QuaternionObj { Value = new Quaternion(x, y, z, w) };
            return BoxedValue.FromObject(qObj);
        }
    }

    /// <summary>
    /// eular(x, y, z) - create Quaternion from Euler angles
    /// </summary>
    internal sealed class EularExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 3)
                throw new Exception("Expected: eular(x, y, z)");

            float x = operands[0].GetFloat();
            float y = operands[1].GetFloat();
            float z = operands[2].GetFloat();
            // Use axis-angle or rotation matrix conversion for Euler -> Quaternion
            Quaternion q = QuaternionFromEuler(x, y, z);
            QuaternionObj qObj = new QuaternionObj { Value = q };
            return BoxedValue.FromObject(qObj);
        }

        private static Quaternion QuaternionFromEuler(float x, float y, float z)
        {
            // Convert Euler angles (degrees) to quaternion
            float rx = x * (float)Math.PI / 180.0f * 0.5f;
            float ry = y * (float)Math.PI / 180.0f * 0.5f;
            float rz = z * (float)Math.PI / 180.0f * 0.5f;
            float sinX = (float)Math.Sin(rx), cosX = (float)Math.Cos(rx);
            float sinY = (float)Math.Sin(ry), cosY = (float)Math.Cos(ry);
            float sinZ = (float)Math.Sin(rz), cosZ = (float)Math.Cos(rz);
            return new Quaternion(
                sinX * cosY * cosZ - cosX * sinY * sinZ,
                cosX * sinY * cosZ + sinX * cosY * sinZ,
                cosX * cosY * sinZ - sinX * sinY * cosZ,
                cosX * cosY * cosZ + sinX * sinY * sinZ);
        }
    }

    /// <summary>
    /// color(r, g, b, a) - create ColorF
    /// </summary>
    internal sealed class ColorExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 4)
                throw new Exception("Expected: color(r, g, b, a)");

            float r = operands[0].GetFloat();
            float g = operands[1].GetFloat();
            float b = operands[2].GetFloat();
            float a = operands[3].GetFloat();
            ColorObj cObj = new ColorObj { Value = new ColorF(r, g, b, a) };
            return BoxedValue.FromObject(cObj);
        }
    }

    /// <summary>
    /// color32(r, g, b, a) - create Color32
    /// </summary>
    internal sealed class Color32Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 4)
                throw new Exception("Expected: color32(r, g, b, a)");

            byte r = (byte)operands[0].GetInt();
            byte g = (byte)operands[1].GetInt();
            byte b = (byte)operands[2].GetInt();
            byte a = (byte)operands[3].GetInt();
            Color32Obj cObj = new Color32Obj { Value = new Color32(r, g, b, a) };
            return BoxedValue.FromObject(cObj);
        }
    }

    // ========== Integer Vectors ==========

    /// <summary>
    /// vector2int(x, y) - create Vector2Int
    /// </summary>
    internal sealed class Vector2IntExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: vector2int(x, y)");

            int x = operands[0].GetInt();
            int y = operands[1].GetInt();
            Vector2IntObj vecObj = new Vector2IntObj { Value = new Vector2Int(x, y) };
            return BoxedValue.FromObject(vecObj);
        }
    }

    /// <summary>
    /// vector3int(x, y, z) - create Vector3Int
    /// </summary>
    internal sealed class Vector3IntExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 3)
                throw new Exception("Expected: vector3int(x, y, z)");

            int x = operands[0].GetInt();
            int y = operands[1].GetInt();
            int z = operands[2].GetInt();
            Vector3IntObj vecObj = new Vector3IntObj { Value = new Vector3Int(x, y, z) };
            return BoxedValue.FromObject(vecObj);
        }
    }

    // ========== Vector List Parsing ==========

    /// <summary>
    /// vector2list(str_split_by_sep) - parse string into list of Vector2 (2 elements per vector)
    /// </summary>
    internal sealed class Vector2ListExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: vector2list(str_split_by_sep)");

            string str = operands[0].ToString();
            var list = Converter.ConvertVector2DList(str);
            return BoxedValue.FromObject(new List<Vector2>(list));
        }
    }

    /// <summary>
    /// vector3list(str_split_by_sep) - parse string into list of Vector3 (3 elements per vector)
    /// </summary>
    internal sealed class Vector3ListExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: vector3list(str_split_by_sep)");

            string str = operands[0].ToString();
            var list = Converter.ConvertVector3DList(str);
            return BoxedValue.FromObject(new List<Vector3>(list));
        }
    }

    // ========== Vector Math ==========

    /// <summary>
    /// vector2dist(pt1, pt2) - distance between two Vector2 points
    /// </summary>
    internal sealed class Vector2DistExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: vector2dist(pt1, pt2)");

            Vector2Obj pt1Obj = operands[0];
            Vector2Obj pt2Obj = operands[1];
            Vector2 pt1 = pt1Obj;
            Vector2 pt2 = pt2Obj;
            Vector2 diff = pt1 - pt2;
            return (float)Math.Sqrt(diff.X * diff.X + diff.Y * diff.Y);
        }
    }

    /// <summary>
    /// vector3dist(pt1, pt2) - distance between two Vector3 points
    /// </summary>
    internal sealed class Vector3DistExp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: vector3dist(pt1, pt2)");

            Vector3Obj pt1Obj = operands[0];
            Vector3Obj pt2Obj = operands[1];
            Vector3 pt1 = pt1Obj;
            Vector3 pt2 = pt2Obj;
            Vector3 diff = pt1 - pt2;
            return (float)Math.Sqrt(diff.X * diff.X + diff.Y * diff.Y + diff.Z * diff.Z);
        }
    }

    /// <summary>
    /// vector2to3(pt) - convert Vector2(x,y) to Vector3(x, 0, y)
    /// </summary>
    internal sealed class Vector2To3Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: vector2to3(pt)");

            Vector2Obj ptObj = operands[0];
            Vector2 pt = ptObj;
            Vector3Obj vecObj = new Vector3Obj { Value = new Vector3(pt.X, 0, pt.Y) };
            return BoxedValue.FromObject(vecObj);
        }
    }

    /// <summary>
    /// vector3to2(pt) - convert Vector3(x,y,z) to Vector2(x, y)
    /// </summary>
    internal sealed class Vector3To2Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 1)
                throw new Exception("Expected: vector3to2(pt)");

            Vector3Obj ptObj = operands[0];
            Vector3 pt = ptObj;
            Vector2Obj vecObj = new Vector2Obj { Value = new Vector2(pt.X, pt.Y) };
            return BoxedValue.FromObject(vecObj);
        }
    }

    // ========== Random Vectors ==========

    internal static class VectorRndUtil
    {
        internal static readonly Random s_Random = new Random();
    }

    /// <summary>
    /// rndvector3(pt, radius) - random Vector3 offset within radius (horizontal plane)
    /// </summary>
    internal sealed class RndVector3Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: rndvector3(pt, radius)");

            Vector3Obj ptObj = operands[0];
            Vector3 pt = ptObj;
            float r = operands[1].GetFloat();

            float deltaX = (float)(VectorRndUtil.s_Random.NextDouble() - 0.5) * r;
            float deltaZ = (float)(VectorRndUtil.s_Random.NextDouble() - 0.5) * r;
            Vector3 result = new Vector3(pt.X + deltaX, pt.Y, pt.Z + deltaZ);

            Vector3Obj vecObj = new Vector3Obj { Value = result };
            return BoxedValue.FromObject(vecObj);
        }
    }

    /// <summary>
    /// rndvector2(pt, radius) - random Vector2 offset within radius
    /// </summary>
    internal sealed class RndVector2Exp : SimpleExpressionBase
    {
        protected override BoxedValue OnCalc(IList<BoxedValue> operands)
        {
            if (operands.Count < 2)
                throw new Exception("Expected: rndvector2(pt, radius)");

            Vector2Obj ptObj = operands[0];
            Vector2 pt = ptObj;
            float r = operands[1].GetFloat();

            float deltaX = (float)(VectorRndUtil.s_Random.NextDouble() - 0.5) * r;
            float deltaY = (float)(VectorRndUtil.s_Random.NextDouble() - 0.5) * r;
            Vector2 result = new Vector2(pt.X + deltaX, pt.Y + deltaY);

            Vector2Obj vecObj = new Vector2Obj { Value = result };
            return BoxedValue.FromObject(vecObj);
        }
    }
}
